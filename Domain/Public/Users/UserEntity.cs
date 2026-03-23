using System.Net;

using Domain.Common.Enums;
using Domain.Common.Exceptions;
using Domain.Public.Users;
using Domain.System.RefreshToken;

namespace SLAIS.Domain.Users;

public class UserEntity : UserNavigationPropertyEntity
{
    public string Email { get; private set; }

    public string HashedPassword { get; private set; }

    public string Username { get; private set; }

    public string FirstName { get; private set; }

    public string LastName { get; private set; }

    public Roles Role { get; private set; }

    public short LoginAttempts { get; private set; }

    public bool IsBlocked { get; private set; }

    public States State { get; private set; }

    public Guid InstituteUuid { get; private set; }

    private UserEntity(
        Guid? createdByUserGuid,
        string email,
        string hashedPassword,
        string username,
        string firstName,
        string lastName,
        Roles role,
        Guid instituteUuid)
        : base(createdByUserGuid)
    {
        Email = email;
        HashedPassword = hashedPassword;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        LoginAttempts = 0;
        IsBlocked = false;
        State = States.Active;
        InstituteUuid = instituteUuid;
    }

    public static UserEntity CreateAdmin(
        Guid? createdByUserGuid,
        string email,
        string hashedPassword,
        string username,
        string firstName,
        string lastName,
        Guid instituteGuid)
    {
        CheckInputs(
            email,
            hashedPassword,
            username,
            firstName,
            lastName);

        return new UserEntity(
            createdByUserGuid,
            email,
            hashedPassword,
            username,
            firstName,
            lastName,
            Roles.Admin,
            instituteGuid
        );
    }

    public void IncrementWrongLoginAttempts(int maxLoginAttempts = 5)
    {
        if (IsBlocked)
        {
            throw new SlaisException(UserErrorCodes.UserIsAlreadyBlocked);
        }

        LoginAttempts++;

        if (LoginAttempts == maxLoginAttempts)
        {
            IsBlocked = true;
        }
    }

    public void SetLoginAttemptsToZero()
    {
        LoginAttempts = 0;
    }

    public void SetPassword(string hashedPassword)
    {
        if (string.IsNullOrWhiteSpace(hashedPassword)
            || hashedPassword == HashedPassword)
        {
            throw new SlaisException(UserErrorCodes.InvalidPassword);
        }

        HashedPassword = hashedPassword;
    }

    public RefreshTokenEntity CreateRefreshToken(
        int expiresInDays,
        Guid deviceGuid,
        string deviceName,
        IPAddress ipAddress)
    {
        var refreshToken = RefreshTokenEntity.CreateRefreshToken(
            expiresInDays,
            deviceGuid,
            deviceName,
            ipAddress,
            Guid);

        RefreshTokens.Add(refreshToken);

        return refreshToken;
    }

    #region Private

    private static void CheckInputs(
        string email,
        string hashedPassword,
        string username,
        string firstName,
        string lastName)
    {
        if (string.IsNullOrWhiteSpace(email)
            || !email.Contains('@')
            || email.Length < 6)
        {
            throw new SlaisException(UserErrorCodes.InvalidInput);
        }

        if (string.IsNullOrWhiteSpace(username)
            || string.IsNullOrWhiteSpace(email)
            || string.IsNullOrWhiteSpace(firstName)
            || string.IsNullOrWhiteSpace(lastName)
            || string.IsNullOrWhiteSpace(hashedPassword))
        {
            throw new SlaisException(UserErrorCodes.InvalidInput);
        }

        if (username.Length < 3
            || username.Length > 100)
        {
            throw new SlaisException(UserErrorCodes.InvalidInput);
        }
    }

    #endregion
}
