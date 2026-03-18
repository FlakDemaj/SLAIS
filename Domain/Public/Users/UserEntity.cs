using SLAIS.Domain.Commom.Enums;

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

    public UserEntity(
        Guid createdByUserGuid,
        string email,
        string hashedPassword,
        string username,
        string firstName,
        string lastName,
        Roles role,
        short loginAttempts,
        bool isBlocked,
        States state,
        Guid instituteUuid)
        : base(createdByUserGuid)
    {
        Email = email;
        HashedPassword = hashedPassword;
        Username = username;
        FirstName = firstName;
        LastName = lastName;
        Role = role;
        LoginAttempts = loginAttempts;
        IsBlocked = isBlocked;
        State = state;
        InstituteUuid = instituteUuid;
    }

    public void IncrementWrongLoginAttempts(int maxLoginAttempts = 5)
    {
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
        HashedPassword = hashedPassword;
    }
}
