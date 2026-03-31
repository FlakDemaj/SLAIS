using System.Net;

using Application.Common.Authentication;
using Application.Common.DTOs.Base;
using Application.Public.Users;
using Application.Public.Users.Commands.GetUsers;

using Domain.Common.Enums;

using FluentAssertions;

using NSubstitute;

using Presentation.Tests.Common;

using Xunit;

namespace Presentation.Tests.Public;

public class UserControllerTest : TestBase
{
    public UserControllerTest(TestWebApplicationFactory factory)
        : base(factory)
    {
    }

    #region GetUsers

    [Fact]
    public async Task GetUsers_ShouldReturnOk_WhenRequestIsValid()
    {
        var expectedResult = new List<GetUserResponseDto>
        {
            new()
            {
                Username = "john.doe",
                Email = "john@doe.de",
                Role = Roles.Admin,
                CreatedBy = new BaseAuditCreatedDto
                {
                    CreatedByFirstname = "Max",
                    CreatedByLastname = "Mustermann",
                    CreationDate = DateTime.UtcNow
                },
                UpdatedBy = null,
                DeletedBy = null
            },
            new()
            {
                Username = "jane.doe",
                Email = "jane@doe.de",
                Role = Roles.Teacher,
                CreatedBy = new BaseAuditCreatedDto
                {
                    CreatedByFirstname = "Max",
                    CreatedByLastname = "Mustermann",
                    CreationDate = DateTime.UtcNow
                },
                UpdatedBy = null,
                DeletedBy = null
            }
        };

        _factory.MediatorMock
            .SendAsync(
                Arg.Any<GetUsersCommand>(),
                Arg.Any<CancellationToken>(),
                Arg.Any<IAuthentication>())
            .Returns(expectedResult);

        AuthenticateAs(
            Roles.Admin,
            Guid.CreateVersion7(),
            Guid.CreateVersion7());

        var response = await _client.GetAsync(Routings.RestUserRouting);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DeserializeResponseAsync<List<GetUserResponseDto>>(response);

        result.Should().NotBeNull();
        result.Should().HaveCount(2);

        result[0].Username.Should().Be(expectedResult[0].Username);
        result[0].Email.Should().Be(expectedResult[0].Email);
        result[0].Role.Should().Be(expectedResult[0].Role);
        result[0].CreatedBy.Should().NotBeNull();
        result[0].CreatedBy.CreatedByFirstname.Should().Be(expectedResult[0].CreatedBy.CreatedByFirstname);
        result[0].CreatedBy.CreatedByLastname.Should().Be(expectedResult[0].CreatedBy.CreatedByLastname);
        result[0].UpdatedBy.Should().BeNull();
        result[0].DeletedBy.Should().BeNull();

        result[1].Username.Should().Be(expectedResult[1].Username);
        result[1].Email.Should().Be(expectedResult[1].Email);
        result[1].Role.Should().Be(expectedResult[1].Role);
        result[1].CreatedBy.Should().NotBeNull();
        result[1].CreatedBy.CreatedByFirstname.Should().Be(expectedResult[1].CreatedBy.CreatedByFirstname);
        result[1].CreatedBy.CreatedByLastname.Should().Be(expectedResult[1].CreatedBy.CreatedByLastname);
        result[1].UpdatedBy.Should().BeNull();
        result[1].DeletedBy.Should().BeNull();
    }

    [Fact]
    public async Task GetUsers_ShouldReturnUnauthorized_WhenUserRequestIsStudent()
    {
        var result = new List<GetUserResponseDto>
        {
            new()
            {
                Username = "john.doe",
                Email = "john@doe.de",
                Role = Roles.Admin,
                CreatedBy = new BaseAuditCreatedDto
                {
                    CreatedByFirstname = "Max",
                    CreatedByLastname = "Mustermann",
                    CreationDate = DateTime.UtcNow
                },
                UpdatedBy = null,
                DeletedBy = null
            },
            new()
            {
                Username = "jane.doe",
                Email = "jane@doe.de",
                Role = Roles.Teacher,
                CreatedBy = new BaseAuditCreatedDto
                {
                    CreatedByFirstname = "Max",
                    CreatedByLastname = "Mustermann",
                    CreationDate = DateTime.UtcNow
                },
                UpdatedBy = null,
                DeletedBy = null
            }
        };

        _factory.MediatorMock
            .SendAsync(
                Arg.Any<GetUsersCommand>(),
                Arg.Any<CancellationToken>(),
                Arg.Any<IAuthentication>())
            .Returns(result);

        AuthenticateAs(
            Roles.Student,
            Guid.CreateVersion7(),
            Guid.CreateVersion7());

        var response = await _client.GetAsync(Routings.RestUserRouting);

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task GetUsers_ShouldReturnOk_WhenUserListIsEmpty()
    {
        var expectedResult = new List<GetUserResponseDto>();

        _factory.MediatorMock
            .SendAsync(
                Arg.Any<GetUsersCommand>(),
                Arg.Any<CancellationToken>(),
                Arg.Any<IAuthentication>())
            .Returns(expectedResult);

        AuthenticateAs(
            Roles.Admin,
            Guid.CreateVersion7(),
            Guid.CreateVersion7());

        var response = await _client.GetAsync(Routings.RestUserRouting);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await DeserializeResponseAsync<List<GetUserResponseDto>>(response);

        result.Should().BeEmpty();
    }

    #endregion
}
