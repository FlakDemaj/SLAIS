using Domain.Common.Enums;
using Domain.Public.Users;
using Domain.Tests.Utils.Extensions;

using FluentAssertions;

using SLAIS.Domain.Users;

using Xunit;

namespace Domain.Tests.Public.Users;

public class UserTests : UserTestBase
{

    #region Create

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
    public void CreateUser_ShouldThrowException_WhenEmailIsInvalid()
    {
        var email = "test";

        var act = () => UserEntity.CreateAdmin(
            Guid.CreateVersion7(),
            email,
            "HashedPassword",
            "testAdmin",
            "Max",
            "Mustermann",
            Guid.CreateVersion7());

        act.ThrowsException(
            UserErrorCodes.InvalidInput);
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUsernameIsWhitespace()
    {
        var username = " ";

        var act = () => UserEntity.CreateAdmin(
            Guid.CreateVersion7(),
            "test@slais.de",
            "HashedPassword",
            username,
            "Max",
            "Mustermann",
            Guid.CreateVersion7());

        act.ThrowsException(
            UserErrorCodes.InvalidInput);
    }

    [Fact]
    public void CreateUser_ShouldThrowException_WhenUsernameIsToSmall()
    {
        var username = "a";

        var act = () => UserEntity.CreateAdmin(
            Guid.CreateVersion7(),
            "test@slais.de",
            "HashedPassword",
            username,
            "Max",
            "Mustermann",
            Guid.CreateVersion7());

        act.ThrowsException(
            UserErrorCodes.InvalidInput);
    }

    #endregion

    #region SetPassword

    [Fact]
    public void SetPassword_ShouldSetCorrectPassword()
    {
        var newPassword = "NewPassword";

        // Act
        _user.SetPassword(newPassword);

        //Assert
        _user.HashedPassword.Should().Be(newPassword);
    }

    [Fact]
    public void SetPassword_ShouldThrowException_WhenPasswordIsInvalid()
    {
        var newPassword = " ";

        // Act
        var act = () => _user.SetPassword(newPassword);

        //Assert
        act.ThrowsException(UserErrorCodes.InvalidPassword);
    }

    [Fact]
    public void SetPassword_ShouldThrowException_PasswordIsTheSame()
    {
        // Act
        var act = () => _user.SetPassword(_user.HashedPassword);

        //Assert
        act.ThrowsException(UserErrorCodes.InvalidPassword);
    }

    #endregion
}
