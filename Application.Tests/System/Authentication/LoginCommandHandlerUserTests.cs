using System.Net;

using Application.Authentication;
using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Common.Interfaces.Services;
using Application.Common.Options;
using Application.Interfaces;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;

using Domain.Common.Exceptions;

using FluentAssertions;

using Microsoft.Extensions.Options;

using NSubstitute;

using SLAIS.Domain.Users;

using Tests.Shared.TestDataCreator;

using Xunit;

namespace Application.Tests.System.Authentication;

public class LoginCommandHandlerUserTests
{
    private readonly IUserRepository _userRepository;

    private readonly ITokenService _tokenService;

    private readonly IPasswordHasher _passwordHasher;

    private readonly IUnitOfWork _unitOfWork;

    private readonly LoginCommandHandlerUser _handlerUser;

    public LoginCommandHandlerUserTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenService = Substitute.For<ITokenService>();
        _passwordHasher = Substitute.For<IPasswordHasher>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ISlaisLogger<LoginCommandHandlerUser>>();

        var refreshTokenOptions = Options.Create(new RefreshTokenOptions
        {
            ExpiresInDays = 7
        });

        var commonOptions = Options.Create(new CommonOptions
        {
            MaxLoginAttempts = 5
        });

        _handlerUser = new LoginCommandHandlerUser(
            _userRepository,
            refreshTokenOptions,
            _tokenService,
            logger,
            _passwordHasher,
            commonOptions,
            _unitOfWork);
    }

    #region HandleAsync - Success

    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var command = BuildValidCommand();
        var user = UserTestData.CreateUser();

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

        var result = await _handlerUser.HandleAsync(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.GeneratedAccessToken.Should().Be(expectedResult);
        result.RefreshToken.Should().NotBeNull();
    }

    #endregion

    #region HandleAsync - User Checks

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenUserDoesNotExist()
    {
        var command = BuildValidCommand();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns((UserEntity?)null);

        var act = async () =>
        {
            return await _handlerUser.HandleAsync(command, CancellationToken.None);
        };

        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.NoUserWithThisName);
    }

    #endregion

    #region HandleAsync - Password Checks

    [Fact]
    public async Task HandleAsync_ShouldThrowException_WhenPasswordIsWrong()
    {
        var command = BuildValidCommand();
        var user = UserTestData.CreateUser();

        _userRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(command.LoginName)
            .Returns(user);

        _passwordHasher
            .Verify(command.Password, user.HashedPassword)
            .Returns(false);

        var act = async () =>
        {
            return await _handlerUser.HandleAsync(command, CancellationToken.None);
        };

        await act.Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.WrongPassword);

        await _unitOfWork
            .Received(1)
            .SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task HandleAsync_ShouldResetLoginAttempts_WhenPasswordIsCorrect()
    {
        var command = BuildValidCommand();
        var user = UserTestData.CreateUserWithLoginAttempts(3);

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

        await _handlerUser.HandleAsync(command, CancellationToken.None);

        user.LoginAttempts.Should().Be(0);
    }

    #endregion

    #region Helpers

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

    #endregion
}
