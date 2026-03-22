using SLAIS.Domain.Commom;
using SLAIS.Domain.Users;

namespace Domain.Institutes;

public abstract class InstituteNavigationPropertyEntity : BaseIdEntity
{
    //Navigation Property to Users
    public ICollection<UserEntity> Users { get; private set; } = new List<UserEntity>();

    protected InstituteNavigationPropertyEntity
        (Guid? createdByUserGuid)
        : base(createdByUserGuid)
    {
    }
}
