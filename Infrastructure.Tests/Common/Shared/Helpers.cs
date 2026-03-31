using Domain.Institutes;
using Domain.System.RefreshToken;

using FluentAssertions;

using SLAIS.Domain.Users;

namespace Infrastructure.Tests.Common.Shared;

public static class Helpers
{

    #region User

    public static void CheckCreatedUser(
        UserEntity? createdUser,
        UserEntity user,
        Guid instituteGuid)
    {
        createdUser.Should().NotBeNull();

        createdUser.Guid.Should().Be(user.Guid);
        createdUser.Id.Should().NotBeNull();
        createdUser.Email.Should().Be(user.Email);
        createdUser.FirstName.Should().Be(user.FirstName);
        createdUser.LastName.Should().Be(user.LastName);
        createdUser.Username.Should().Be(user.Username);
        createdUser.State.Should().Be(user.State);
        createdUser.Role.Should().Be(user.Role);
        createdUser.HashedPassword.Should().Be(user.HashedPassword);
        createdUser.IsBlocked.Should().Be(user.IsBlocked);
        createdUser.LoginAttempts.Should().Be(user.LoginAttempts);
        createdUser.InstituteGuid.Should().Be(instituteGuid);

        createdUser.CreatedDate.Should().BeCloseTo(
            user.CreatedDate,
            precision: TimeSpan.FromMicroseconds(1));
        createdUser.CreatedByUserGuid.Should().Be(user.CreatedByUserGuid);

        createdUser.DeleteDate.Should().Be(user.DeleteDate);
        createdUser.DeletedByUserGuid.Should().Be(user.DeletedByUserGuid);

        createdUser.UpdateDate.Should().Be(user.UpdateDate);
        createdUser.UpdatedByUserGuid.Should().Be(user.UpdatedByUserGuid);
    }

    public static void CheckCreatedRefreshToken(
        RefreshTokenEntity? refreshToken,
        UserEntity user)
    {
        refreshToken.Should().NotBeNull();

        refreshToken.Guid.Should().NotBeEmpty();
        refreshToken.RefreshToken.Should().NotBeEmpty();
        refreshToken.CreatedDate.Should().BeAfter(user.CreatedDate);
        refreshToken.Revoked.Should().BeFalse();
        refreshToken.ExpirationDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(7),
            precision: TimeSpan.FromSeconds(1));
        refreshToken.DeviceGuid.Should().NotBeEmpty();
        refreshToken.DeviceName.Should().NotBeEmpty();
        refreshToken.UserGuid.Should().Be(user.Guid);
        refreshToken.IpAddress.Should().NotBeNull();
        refreshToken.LastUsedDate.Should().BeBefore(DateTime.UtcNow);
        refreshToken.RevokedDate.Should().BeNull();
    }

    #endregion

    #region Institutes

    public static void CheckCreatedInstitute(
        InstituteEntity? createdInstitute,
        InstituteEntity institute)
    {
        createdInstitute.Should().NotBeNull();

        createdInstitute.Guid.Should().Be(institute.Guid);
        createdInstitute.Name.Should().Be(institute.Name);
        createdInstitute.Branch.Should().Be(institute.Branch);
        createdInstitute.Id.Should().NotBeNull();

        createdInstitute.UpdatedByUserGuid.Should().Be(institute.UpdatedByUserGuid);
        createdInstitute.UpdateDate.Should().Be(institute.UpdateDate);

        createdInstitute.DeletedByUserGuid.Should().Be(institute.DeletedByUserGuid);
        createdInstitute.DeleteDate.Should().Be(institute.DeleteDate);

        createdInstitute.CreatedByUserGuid.Should().Be(institute.CreatedByUserGuid);
        createdInstitute.CreatedDate.Should().BeCloseTo(
            institute.CreatedDate,
            precision: TimeSpan.FromSeconds(1));
    }

    #endregion
}
