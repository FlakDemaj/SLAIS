using Application.Common.Mappers.Public;
using Application.Interfaces;
using Application.Public.Users;
using Application.Public.Users.Querys.GetUser;
using Application.Utils.Logger;

using AutoMapper;

using Domain.Common.Enums;
using Domain.Common.Exceptions;

using FluentAssertions;

using NSubstitute;

using SLAIS.Domain.Users;

using Tests.Shared.TestDataCreator;

using Xunit;

namespace Application.Tests.Public.Users.Querys;

public class GetUserQueryHandlerTests
{
    private readonly IUserRepository _userRepository;
    private readonly GetUserQueryHandler _getUserQueryHandler;

    public GetUserQueryHandlerTests()
    {
        _userRepository = Substitute.For<IUserRepository>();

        var logger = Substitute.For<ISlaisLogger<GetUserQuery>>();

        var mapper = new MapperConfiguration(config =>
        {
            config.AddProfile<UserMappingProfile>();
        }).CreateMapper();

        _getUserQueryHandler = new GetUserQueryHandler(
            _userRepository,
            mapper,
            logger);
    }

    #region UserNotFound

    [Fact]
    public async Task GetUser_ShouldThrowUserNotFound_WhenUserDoesNotExist()
    {
        var targetUser = UserTestData.CreateUser();

        _userRepository
            .GetUserByGuidAsync(Arg.Any<Guid>())
            .Returns((UserEntity?)null);

        var authentication = new Common.Authentication.Authentication(
            Guid.CreateVersion7(),
            Roles.Admin,
            Guid.CreateVersion7());

        var request = new GetUserQuery
        {
            Guid = targetUser.Guid
        };

        var act = async () =>
        {
            return await _getUserQueryHandler.HandleAsync(
                        request,
                        authentication,
                        CancellationToken.None);
        };

        await act.Should().ThrowAsync<SlaisException>()
            .Where(e => e.ErrorCode == (int)UserErrorCodes.UserNotFound);
    }

    #endregion

    #region Forbidden

    [Theory]
    [InlineData(Roles.Student, Roles.Student)]
    [InlineData(Roles.Student, Roles.Teacher)]
    [InlineData(Roles.Student, Roles.Admin)]
    [InlineData(Roles.Student, Roles.SuperAdmin)]
    [InlineData(Roles.Teacher, Roles.Teacher)]
    [InlineData(Roles.Teacher, Roles.Admin)]
    [InlineData(Roles.Teacher, Roles.SuperAdmin)]
    [InlineData(Roles.Admin, Roles.Admin)]
    [InlineData(Roles.Admin, Roles.SuperAdmin)]
    public async Task GetUser_ShouldThrowForbidden_WhenCallerIsNotAllowedToViewTargetRole(
        Roles callerRole,
        Roles targetRole)
    {
        var targetUser = UserTestData.CreateUser(roles: targetRole);

        _userRepository
            .GetUserByGuidAsync(Arg.Any<Guid>())
            .Returns(targetUser);

        var authentication = new Common.Authentication.Authentication(
            Guid.CreateVersion7(),
            callerRole,
            Guid.CreateVersion7());

        var request = new GetUserQuery
        {
            Guid = targetUser.Guid
        };

        var act = async () =>
        {
            return await _getUserQueryHandler.HandleAsync(
                        request,
                        authentication,
                        CancellationToken.None);
        };

        await act.Should().ThrowAsync<SlaisException>()
            .Where(e => e.ErrorCode == (int)UserErrorCodes.Forbidden);
    }

    #endregion

    #region OK - Erlaubte Rollenkombinationen

    [Theory]
    [InlineData(Roles.SuperAdmin, Roles.Student)]
    [InlineData(Roles.SuperAdmin, Roles.Teacher)]
    [InlineData(Roles.SuperAdmin, Roles.Admin)]
    [InlineData(Roles.SuperAdmin, Roles.SuperAdmin)]
    [InlineData(Roles.Server, Roles.Student)]
    [InlineData(Roles.Server, Roles.Teacher)]
    [InlineData(Roles.Server, Roles.Admin)]
    [InlineData(Roles.Server, Roles.SuperAdmin)]
    [InlineData(Roles.Admin, Roles.Student)]
    [InlineData(Roles.Admin, Roles.Teacher)]
    [InlineData(Roles.Teacher, Roles.Student)]
    public async Task GetUser_ShouldReturnUser_WhenCallerIsAllowedToViewTargetRole(
        Roles callerRole,
        Roles targetRole)
    {
        var targetUser = UserTestData.CreateUser(roles: targetRole);

        _userRepository
            .GetUserByGuidAsync(Arg.Any<Guid>())
            .Returns(targetUser);

        var authentication = new Common.Authentication.Authentication(
            Guid.CreateVersion7(),
            callerRole,
            Guid.CreateVersion7());

        var request = new GetUserQuery
        {
            Guid = targetUser.Guid
        };

        var result = await _getUserQueryHandler.HandleAsync(
            request,
            authentication,
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Username.Should().Be(targetUser.Username);
        result.Email.Should().Be(targetUser.Email);
        result.Role.Should().Be(targetUser.Role);
        result.State.Should().Be(targetUser.State);
    }

    #endregion

    #region OK - Attribute vollständig prüfen

    [Fact]
    public async Task GetUser_ShouldReturnAllAttributes_WhenRequestIsValid()
    {
        var createdByUser = UserTestData.CreateUser();
        var targetUser = UserTestData.CreateUser(
            roles: Roles.Student,
            createdByUser: createdByUser);

        _userRepository
            .GetUserByGuidAsync(Arg.Any<Guid>())
            .Returns(targetUser);

        var authentication = new Common.Authentication.Authentication(
            Guid.CreateVersion7(),
            Roles.Admin,
            Guid.CreateVersion7());

        var request = new GetUserQuery
        {
            Guid = targetUser.Guid
        };

        var result = await _getUserQueryHandler.HandleAsync(
            request,
            authentication,
            CancellationToken.None);

        result.Should().NotBeNull();
        result.Username.Should().Be(targetUser.Username);
        result.Email.Should().Be(targetUser.Email);
        result.Role.Should().Be(targetUser.Role);
        result.State.Should().Be(targetUser.State);

        result.BaseAuditCreated.Should().NotBeNull();
        result.BaseAuditCreated.CreationDate.Should().BeCloseTo(targetUser.CreatedDate, TimeSpan.FromSeconds(1));
        result.BaseAuditCreated.CreatedByFirstname.Should().Be(createdByUser.FirstName);
        result.BaseAuditCreated.CreatedByLastname.Should().Be(createdByUser.LastName);

        result.BaseAuditUpdated.Should().BeNull();
        result.BaseAuditDeleted.Should().BeNull();
    }

    #endregion
}
