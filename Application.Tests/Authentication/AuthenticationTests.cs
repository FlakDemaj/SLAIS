using Domain.Common.Enums;

using FluentAssertions;

using Xunit;

namespace Domain.Tests.Authentication;

public class AuthenticationTests
{
    [Fact]
    public void Authentication_ShouldSetCorrectValues_WhenCreated()
    {
        var userGuid = Guid.NewGuid();
        var instituteGuid = Guid.NewGuid();
        var userRole = Roles.Admin;

        var authentication = new Application.Common.Authentication.Authentication(userGuid, userRole, instituteGuid);

        authentication.UserGuid.Should().Be(userGuid);
        authentication.UserRole.Should().Be(userRole);
        authentication.InstitutionGuid.Should().Be(instituteGuid);
    }
}
