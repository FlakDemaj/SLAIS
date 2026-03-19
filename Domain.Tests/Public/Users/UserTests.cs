using FluentAssertions;

using SLAIS.Domain.Commom.Enums;

using Xunit;

namespace Domain.Tests.Public.Users;

public class UserTests : UserTestBase
{
    public UserTests()
    {
    }

    [Fact]
    public void CreateAdmin_ShouldSetCorrectEmail()
    {
        _user.Email.Should().Be("test@slais.de");
    }

    [Fact]
    public void CreateAdmin_ShouldSetCorrectName()
    {
        _user.FirstName.Should().Be("Max");
        _user.LastName.Should().Be("Mustermann");
    }

    [Fact]
    public void CreateAdmin_ShouldSetCorrectPassword()
    {
        _user.HashedPassword.Should().Be("HashedPassword");
    }

    [Fact]
    public void CreateAdmin_ShouldSetCorrectUserName()
    {
        _user.Username.Should().Be("testAdmin");
    }

    [Fact]
    public void CreateAdmin_ShouldSetCorrectRole()
    {
        _user.Role.Should().Be(Roles.Admin);
    }

    [Fact]
    public void CreateAdmin_ShouldSetDefaultValues()
    {
        _user.LoginAttempts.Should().Be(0);
        _user.IsBlocked.Should().BeFalse();
        _user.State.Should().Be(States.Active);
    }

    [Fact]
    public void CreateAdmin_ShouldSetAuditDefaults()
    {
        _user.CreatedByUserGuid.Should().NotBeNull();
        _user.CreatedDate.Should().BeAfter(DateTime.MinValue);
        _user.UpdateDate.Should().BeNull();
        _user.UpdatedByUserGuid.Should().BeNull();
        _user.DeleteDate.Should().BeNull();
        _user.DeletedByUserGuid.Should().BeNull();
    }

    [Fact]
    public void CreateAdmin_ShouldHaveInstituteGuid()
    {
        _user.InstituteUuid.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateAdmin_ShouldGenerateGuid()
    {
        _user.Guid.Should().NotBeEmpty();
    }

    [Fact]
    public void CreateAdmin_ShouldNotGenerateId()
    {
        _user.Id.Should().BeNull();
    }

    [Fact]
    public void IncrementWrongLoginAttempts_ShouldIncrementLoginAttempts()
    {
        // Act
        _user.IncrementWrongLoginAttempts();

        // Assert
        _user.LoginAttempts.Should().Be(1);
        _user.IsBlocked.Should().BeFalse();
    }

    [Fact]
    public void IncrementWrongLoginAttempts_ShouldBlockUser()
    {
        // Act
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();

        // Assert
        _user.LoginAttempts.Should().Be(5);
        _user.IsBlocked.Should().BeTrue();
    }

    [Fact]
    public void SetLoginAttemptsToZero_ShouldSetCorrectLoginAttempts()
    {
        // Act
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();
        _user.IncrementWrongLoginAttempts();

        _user.SetLoginAttemptsToZero();

        // Assert
        _user.LoginAttempts.Should().Be(0);
    }

    [Fact]
    public void SetPassword_ShouldSetCorrectPassword()
    {
        var newPassword = "NewPassword";

        // Act
        _user.SetPassword(newPassword);

        //Assert
        _user.HashedPassword.Should().Be(newPassword);
    }
}
