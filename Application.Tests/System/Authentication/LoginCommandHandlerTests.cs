using System.Net;

using Application.Authentication;
using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Application.Interfaces;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;

using AutoMapper;

using Domain.Common.Exceptions;
using Domain.Institutes;

using FluentAssertions;

using Microsoft.Extensions.Options;

using NSubstitute;

using SLAIS.Domain.Users;

using Xunit;

namespace Application.Tests.System.Authentication;

public class LoginCommandHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly ITokenService _tokenService;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ISlaisLogger<LoginCommandHandler> _logger;
    private readonly IOptions<CommonOptions> _commonOptions;
    private readonly LoginCommandHandler _handler;

    public LoginCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenService = Substitute.For<ITokenService>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        _logger = Substitute.For<ISlaisLogger<LoginCommandHandler>>();
        var mapper = Substitute.For<IMapper>();

        var refreshTokenOptions = Options.Create(new RefreshTokenOptions
        {
            ExpiresInDays = 7
        });

        _commonOptions = Options.Create(new CommonOptions
        {
            MaxLoginAttempts = 5
        });

        _handler = new LoginCommandHandler(
            _userRepository,
            refreshTokenOptions,
            _tokenService,
            _logger,
            _passwordHasher,
            mapper,
            _commonOptions,
            _unitOfWork);
    }

    #region HandleAsync - Success

    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildValidUser();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        _passwordHasher
            .Verify(command.Password, user.HashedPassword)
            .Returns(true);

        var expectedResult = new GeneratedAccessTokenResult
        {
            AccessToken = "access-token",
            AccessTokenExpiresInMinutes = 900
        };

        _tokenService
            .GenerateAccessToken(user)
            .Returns(expectedResult);

        // Act
        var result = await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result.Should().NotBeNull();
        result.GeneratedAccessToken.Should().Be(expectedResult);
        result.RefreshToken.Should().NotBeNull();
    }

    #endregion

    #region HandleAsync - User Checks

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        // Arrange
        var command = BuildValidCommand();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns((UserEntity?)null);

        // Act
        var act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.NoUserWithThisName);
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenUserIsBlocked()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildBlockedUser();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        // Act
        var act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.UserIsBlocked);
    }

    #endregion

    #region HandleAsync - Password Checks

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenPasswordIsWrong()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildValidUser();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        _passwordHasher
            .Verify(command.Password, user.HashedPassword)
            .Returns(false);

        // Act
        var act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.WrongPassword);

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenUserGetsBlockedAfterWrongPassword()
    {
        // Arrange
        var command = BuildValidCommand();

        // User hat bereits MaxLoginAttempts - 1 Fehlversuche
        var user = BuildUserWithLoginAttempts(_commonOptions.Value.MaxLoginAttempts - 1);

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        _passwordHasher
            .Verify(command.Password, user.HashedPassword)
            .Returns(false);

        // Act
        var act = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.UserIsBlocked);

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldResetLoginAttempts_WhenPasswordIsCorrect()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildUserWithLoginAttempts(3);

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        _passwordHasher
            .Verify(command.Password, user.HashedPassword)
            .Returns(true);

        var expectedResult = new GeneratedAccessTokenResult
        {
            AccessToken = "access-token",
            AccessTokenExpiresInMinutes = 900
        };

        _tokenService
            .GenerateAccessToken(user)
            .Returns(expectedResult);

        // Act
        await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        user.LoginAttempts.Should().Be(0);
    }

    #endregion

    #region Builders

    private static LoginCommand BuildValidCommand()
    {
        return new LoginCommand
        {
            LoginName = "testuser",
            Password = "Test1234!",
            DeviceGuid = Guid.NewGuid(),
            DeviceName = "Test Device",
            IpAddress = IPAddress.Loopback
        };
    }

    private static UserEntity BuildValidUser()
    {
        var institute = InstituteEntity.Create(
            createdByUserGuid: null,
            name: "testInstitute",
            branch: "Health");

        return UserEntity.CreateAdmin(
            instituteGuid: institute.Guid,
            createdByUserGuid: null,
            username: "testuser",
            email: "test@test.com",
            hashedPassword: "hashed-password",
            firstName: "Max",
            lastName: "Mustermann");
    }

    private static UserEntity BuildBlockedUser()
    {
        var user = BuildValidUser();

        user.IncrementWrongLoginAttempts(1);

        return user;
    }

    private static UserEntity BuildUserWithLoginAttempts(int attempts)
    {
        var user = BuildValidUser();

        for (var i = 0; i < attempts; i++)
        {
            user.IncrementWrongLoginAttempts(999);
        }

        return user;
    }

    #endregion
}
