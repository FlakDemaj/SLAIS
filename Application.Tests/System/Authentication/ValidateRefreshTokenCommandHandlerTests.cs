using System.Net;

using Application.Authentication;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.ValidateRefreshToken;
using Application.Common.Interfaces.Services;
using Application.Interfaces;
using Application.Tests.Common;
using Application.Utils.Interfaces.Transaction;
using Application.Utils.Logger;

using ClassLibrary1.Builders;

using Domain.Common.Exceptions;

using FluentAssertions;

using NSubstitute;

using SLAIS.Domain.Users;

using Xunit;

namespace Application.Tests.System.Authentication;

public class ValidateRefreshTokenCommandHandlerTests : TestBase
{
    private readonly IUserRepository _userRepository;

    private readonly ITokenService _tokenService;

    private readonly IUnitOfWork _unitOfWork;

    private readonly ISlaisLogger<ValidateRefreshTokenCommandHandler> _logger;

    private readonly ValidateRefreshTokenCommandHandler _handler;

    public ValidateRefreshTokenCommandHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();
        _tokenService = Substitute.For<ITokenService>();
        _unitOfWork = Substitute.For<IUnitOfWork>();
        var logger = Substitute.For<ISlaisLogger<ValidateRefreshTokenCommandHandler>>();

        _handler = new ValidateRefreshTokenCommandHandler(
            logger,
            _userRepository,
            _unitOfWork,
            _tokenService);
    }

    #region HandleAsync - Success

    [Fact]
    public async Task HandleAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildValidUserWithRefreshTokens(3);
        var newUser = new RefreshTokenEntityBuilder()
            .BuildWithOwnRefreshTokenGuid(user, command.RefreshToken);

        _userRepository
            .GetUserWithRefreshTokensByGuidAsync(command.RefreshToken)
            .Returns(newUser);

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
        result.AccessToken.Should().Be(expectedResult.AccessToken);
        result.AccessTokenExpiresInMinutes.Should().Be(expectedResult.AccessTokenExpiresInMinutes);
    }

    #endregion

    #region Handle Async - Failed

    [Fact]
    public async Task HandleAsync_ShouldReturnNoTokenWithThisUser_WhenTokenIsNotValid()
    {
        // Arrange
        var command = BuildValidCommand();

        _userRepository
            .GetUserWithRefreshTokensByGuidAsync(command.RefreshToken)
            .Returns((UserEntity?)null);

        // Act
        var result = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.NoUserWithThisToken);
    }

    [Fact]
    public async Task HandleAsync_ShouldReturnNoTokenWithThisUser_WhenTokenIsRevoked()
    {
        // Arrange
        var command = BuildValidCommand();
        var user = BuildValidUserWithRefreshTokens(3);
        var newUser = new RefreshTokenEntityBuilder()
            .BuildWithOwnRevokedGuidReturnsUserEntity(user);

        _userRepository
            .GetUserWithRefreshTokensByGuidAsync(command.RefreshToken)
            .Returns(newUser);

        // Act
        var result = async () => await _handler.HandleAsync(command, CancellationToken.None);

        // Assert
        result
            .Should()
            .ThrowAsync<SlaisException>()
            .Where(x => x.ErrorCode == (int)AuthErrorCodes.NoValidTokenFound);
    }

    #endregion

    #region Helpers

    private static ValidateRefreshTokenCommand BuildValidCommand()
    {
        return new ValidateRefreshTokenCommand
        {
            RefreshToken = Guid.CreateVersion7()
        };
    }

    #endregion
}
