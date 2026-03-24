using System.Net;

using Application.Authentication;
using Application.Authentication.Commands;
using Application.Authentication.Commands.Login;

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

    [Fact]
    public async Task Login_ShouldReturnAccessToken_WhenRequestIsValid()
    {
        // Arrange
        var expectedResult = new LoginResponseDto { AccessToken = Guid.NewGuid().ToString() };
        var expectedMediatorResult = new GeneratedTokenResult
        {
            AccessToken = expectedResult.AccessToken,
            ExpiresIn = 10,
            RefreshToken = Guid.CreateVersion7()
        };

        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .Returns(expectedMediatorResult);

        var payload = new LoginRequest
        {
            LoginName = "TestLoginName",
            Password = "123456789!",
            DeviceName = "Samsung Iphone S8 +",
            DeviceGuid = Guid.CreateVersion7()
        };

        var content = BuildJsonContent(payload);

        var response = await _client.PostAsync(Routings.RestAuthenticationRouting + "login", content);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DeserializeResponseAsync<LoginResponseDto>(response);

        result.Should().NotBeNull();
        result.AccessToken.Should().Be(expectedResult.AccessToken);
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserNotFound()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.NoUserWithThisName));

        var payload = new LoginRequest
        {
            LoginName = "TestLoginName",
            Password = "123456789!",
            DeviceName = "Samsung Iphone S8 +",
            DeviceGuid = Guid.CreateVersion7()
        };

        var content = BuildJsonContent(payload);

        // Act
        var response = await _client.PostAsync(Routings.RestAuthenticationRouting + "login", content);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        // Assert
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

        var payload = new LoginRequest
        {
            LoginName = "TestLoginName",
            Password = "123456789!",
            DeviceName = "Samsung Iphone S8 +",
            DeviceGuid = Guid.CreateVersion7()
        };

        var content = BuildJsonContent(payload);

        // Act
        var response = await _client.PostAsync(Routings.RestAuthenticationRouting + "login", content);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.WrongPassword);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.WrongPassword.GetDescription());
    }

    [Fact]
    public async Task Login_ShouldReturnUnauthorized_WhenUserIsBlocked()
    {
        _factory.MediatorMock
            .SendAsync(Arg.Any<LoginCommand>(), Arg.Any<CancellationToken>())
            .ThrowsAsync(new SlaisException(AuthErrorCodes.UserIsBlocked));

        var payload = new LoginRequest
        {
            LoginName = "TestLoginName",
            Password = "123456789!",
            DeviceName = "Samsung Iphone S8 +",
            DeviceGuid = Guid.CreateVersion7()
        };

        var content = BuildJsonContent(payload);

        // Act
        var response = await _client.PostAsync(Routings.RestAuthenticationRouting + "login", content);

        var errorResponse = await DeserializeResponseAsync<ErrorResponseDto>(response);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        errorResponse.Should().NotBeNull();
        errorResponse.ErrorCode.Should().Be((int)AuthErrorCodes.UserIsBlocked);
        errorResponse.ErrorMessage.Should().Be(AuthErrorCodes.UserIsBlocked.GetDescription());
    }


}
