using Domain.Common.Enums;
using Domain.System.RefreshToken;

using SLAIS.Domain.Users;

using Tests.Shared.Builders;

namespace Tests.Shared.TestDataCreator;

public static class UserTestData
{
    public static UserEntity CreateUser(
        UserEntity? createdByUser = null,
        string? email = null,
        string? hashedPassword = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null,
        Roles? roles = null,
        Guid? instituteGuid = null)
    {
        return new UserEntityBuilder()
            .WithEmail(email ?? Guid.CreateVersion7() + "test@slais.de")
            .WithHashedPassword(hashedPassword ?? "HashedPassword")
            .WithUsername(username ?? Guid.CreateVersion7().ToString())
            .WithFirstName(firstName ?? "Max")
            .WithLastName(lastName ?? "Mustermann")
            .WithInstituteGuid(instituteGuid ?? Guid.NewGuid())
            .WithCreatedByUserGuid(createdByUser)
            .WithRole(roles ??  Roles.Teacher)
            .Build();
    }

    public static UserEntity CreateBlockedUser(
        string? email = null,
        string? username = null,
        string? firstName = null,
        string? lastName = null)
    {
        return new UserEntityBuilder()
            .WithEmail(email ?? "test@slais.de")
            .WithUsername(username ?? "testAdmin")
            .WithFirstName(firstName ?? "Max")
            .WithLastName(lastName ?? "Mustermann")
            .WithBlocked()
            .Build();
    }

    public static UserEntity CreateUserWithLoginAttempts(
        short attempts,
        string? email = null,
        string? username = null)
    {
        return new UserEntityBuilder()
            .WithEmail(email ?? "test@slais.de")
            .WithUsername(username ?? "testAdmin")
            .WithLoginAttempts(attempts)
            .Build();
    }

    public static RefreshTokenEntity CreateRefreshToken(
        UserEntity user,
        Guid? refreshTokenGuid = null,
        string? deviceName = null,
        Guid? deviceGuid = null,
        int? expiresInDays = null)
    {
        return new RefreshTokenEntityBuilder()
            .WithDeviceName(deviceName ?? "Test Device")
            .WithDeviceGuid(deviceGuid ?? Guid.NewGuid())
            .WithExpiresInDays(expiresInDays ?? 7)
            .WithRefreshTokenGuid(refreshTokenGuid ?? Guid.CreateVersion7())
            .Build(user);
    }

    public static RefreshTokenEntity CreateExpiredRefreshToken(UserEntity user)
    {
        return new RefreshTokenEntityBuilder()
            .WithExpired()
            .Build(user);
    }

    public static RefreshTokenEntity CreateRevokedRefreshToken(UserEntity user)
    {
        return new RefreshTokenEntityBuilder()
            .WithRevoked()
            .Build(user);
    }
}
