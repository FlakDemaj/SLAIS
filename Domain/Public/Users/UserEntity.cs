using Domain.Institutes;
using Microsoft.AspNetCore.Identity;
using SAIS.Domain.Commom;
using SAIS.Domain.Commom.Enums;

namespace SAIS.Domain.Users;

public class UserEntity : UserNavigationPropertyEntity
{
    public string Email { get; private set; }
    
    public string PasswordHashed { get; private set; }
    
    public string Username { get; private set; }
    
    public string FirstName { get; private set; }
    
    public string LastName { get; private set; }
    
    public Roles Role { get; private set; }
    
    public short LoginAttempts { get; private set; }
    
    public bool IsBlocked { get; private set; }
    
    public States State { get; private set; }
    
    public Guid InstituteUuid { get; private set; }

    public bool VerifyPassword(string password, IPasswordHasher<UserEntity> hasher)
    {
        var result = hasher.VerifyHashedPassword(this, PasswordHashed, password);
        return result == PasswordVerificationResult.Success;
    }

    public void IncrementWrongLoginAttempts()
    {
        LoginAttempts++;

        if (LoginAttempts == 3)
        {
            IsBlocked = true;
        }
    }
}