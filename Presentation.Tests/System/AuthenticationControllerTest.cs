using System.Net;

using Application.Authentication;
using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;
using Application.Authentication.Commands.ValidateRefreshToken;

using Domain.Common.Exceptions;

using FluentAssertions;

using NSubstitute;
using NSubstitute.ExceptionExtensions;

using Presentation.Controllers;
using Presentation.Tests.Common;

using Xunit;

namespace Presentation.Tests.System;

public class AuthenticationControllerTest : TestBase
{
    public AuthenticationControllerTest(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    #region Login

    [Fact]
    public async Task Login_ShouldReturnAccessToken_WhenRequestIsValid()
    {
        var expectedResult = new GeneratedTokenResult
        {
            GeneratedAccessToken =
                new GeneratedAccessTokenResult
                {
                    AccessToken = Guid.CreateVersion7().ToString(),
                    AccessTokenExpiresInMinutes = 900
                },
            RefreshToken = new GeneratedRefreshTokenResult
            {
                RefreshToken = Guid.CreateVersion7(),
                RefreshTokenExpiresInDays = 15
            }
        };

        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        var response = await _client.PostAsync(
            Routings.RestAuthenticationRouting + "login",
            BuildJsonContent(BuildLoginRequest()));

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DeserializeResponseAsync<AccessTokenResponseDto>(response);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.GeneratedAccessToken.AccessToken);

        var setCookieHeader = response.Headers
            .SingleOrDefault(h => h.Key == "Set-Cookie")
            .Value?
            .FirstOrDefault();

        setCookieHeader.Should().NotBeNull();
        setCookieHeader.Should().Contain($"RefreshToken={expectedResult.RefreshToken.RefreshToken}");
        setCookieHeader.Should().Contain("httponly");
        setCookieHeader.Should().Contain("secure");
        setCookieHeader.Should().Contain("samesite=strict");
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.NoUserWithThisName));

        var response = await _client.PostAsync(
            Routings.RestAuthenticationRouting + "login",
            BuildJsonContent(BuildLoginRequest()));

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.NoUserWithThisName);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.NoUserWithThisName.GetDescription());
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsFalse()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.WrongPassword));

        var response = await _client.PostAsync(
            Routings.RestAuthenticationRouting + "login",
            BuildJsonContent(BuildLoginRequest()));

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.WrongPassword);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.WrongPassword.GetDescription());
    }

    #endregion

    #region ValidateRefreshToken

    [Fact]
    public async Task ValidateRefreshToken_ShouldReturnAccessToken_WhenTokenIsValid()
    {
        var expectedResult = new AccessTokenResponseDto
        {
            AccessToken = Guid.NewGuid().ToString(),
            AccessTokenExpiresInMinutes = 900
        };

        _factory.MediatorMock
            .SendAsync(Arg.Any<ValidateRefreshTokenCommand>(), Arg.Any<CancellationToken>())
            .Returns(expectedResult);

        SetRefreshTokenCookie();

        var response = await _client.GetAsync(Routings.RestAuthenticationRouting);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DeserializeResponseAsync<AccessTokenResponseDto>(response);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.AccessToken);
        result.AccessTokenExpiresInMinutes.Should().Be(expectedResult.AccessTokenExpiresInMinutes);
    }

    [Fact]
    public async Task ValidateRefreshToken_ShouldReturnUnauthorized_WhenCookieIsMissing()
    {
        var response = await _client.GetAsync(Routings.RestAuthenticationRouting);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.NoValidTokenFound);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.NoValidTokenFound.GetDescription());
    }

    [Fact]
    public async Task ValidateRefreshToken_ShouldReturnUnauthorized_WhenCookieIsInvalidGuid()
    {
        SetRefreshTokenCookie("kein-gueltiger-guid");

        var response = await _client.GetAsync(Routings.RestAuthenticationRouting);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.NoValidTokenFound);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.NoValidTokenFound.GetDescription());
    }

    [Fact]
    public async Task ValidateRefreshToken_ShouldReturnUnauthorized_WhenTokenHasNoUser()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<ValidateRefreshTokenCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.NoUserWithThisToken));

        SetRefreshTokenCookie();

        var response = await _client.GetAsync(Routings.RestAuthenticationRouting);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.NoUserWithThisToken);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.NoUserWithThisToken.GetDescription());
    }

    [Fact]
    public async Task ValidateRefreshToken_ShouldReturnUnauthorized_WhenTokenIsInvalid()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<ValidateRefreshTokenCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.NoValidTokenFound));

        SetRefreshTokenCookie();

        var response = await _client.GetAsync(Routings.RestAuthenticationRouting);

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.NoValidTokenFound);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.NoValidTokenFound.GetDescription());
    }

    #endregion

    #region Helpers

    private static LoginRequest BuildLoginRequest(
        string? loginName = null,
        string? password = null,
        string? deviceName = null,
        Guid? deviceGuid = null)
    {
        return new LoginRequest
        {
            LoginName = loginName ?? "TestLoginName",
            Password = password ?? "123456789!",
            DeviceName = deviceName ?? "Samsung Iphone S8 +",
            DeviceGuid = deviceGuid ?? Guid.CreateVersion7()
        };
    }

    private void SetRefreshTokenCookie(string? value = null)
    {
        _client.DefaultRequestHeaders.Add(
            "Cookie",
            $"RefreshToken={value ?? Guid.CreateVersion7().ToString()}");
    }

    #endregion
}
