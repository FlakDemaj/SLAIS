using Domain.Institutes;
using SAIS.Domain.Commom;
using SAIS.Domain.Commom.Enums;

namespace SAIS.Domain.Users;

public class UserEntity : BaseIdEntity
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
    
    // Navigation Property for created Users
    public ICollection<UserEntity>? CreatedUsers { get; private set; } = new List<UserEntity>();
    
    //Navigation Property for updated Users
    public ICollection<UserEntity>? UpdatedUsers { get; private set; } = new List<UserEntity>();
    
    //Navigation Property for deleted Users
    public ICollection<UserEntity>? DeletedUsers { get; private set; } = new List<UserEntity>();
    
    //Navigation Property to the institute
    public InstituteEntity Institute { get; private set; }
    
    // Navigation Property for created Users
    public ICollection<InstituteEntity>? CreatedInstitutes { get; private set; } = new List<InstituteEntity>();
    
    //Navigation Property for updated Users
    public ICollection<InstituteEntity>? UpdatedInstitute { get; private set; } = new List<InstituteEntity>();
    
    //Navigation Property for deleted Users
    public ICollection<InstituteEntity>? DeletedInstitute { get; private set; } = new List<InstituteEntity>();
}