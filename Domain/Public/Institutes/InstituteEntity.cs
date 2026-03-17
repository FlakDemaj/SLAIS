using SLAIS.Domain.Commom;
using SLAIS.Domain.Users;

namespace Domain.Institutes;

public class InstituteEntity : BaseIdEntity
{
    public string Name { get; private set; }

    public string Branch { get; private set; }

    //Navigation Property to Users
    public ICollection<UserEntity> Users { get; private set; } = new List<UserEntity>();
}
