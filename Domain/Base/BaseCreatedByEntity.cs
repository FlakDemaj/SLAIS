using SLAIS.Domain.Commom;
using SLAIS.Domain.Users;

namespace Domain.Base;

public abstract class BaseCreatedByEntity : BaseGuidEntity
{
    public Guid? CreatedByUserGuid { get; private set; }

    public DateTime CreatedDate { get; init; }

    // Navigation property
    public UserEntity? CreatedByUser { get; init; }

    protected BaseCreatedByEntity(
        Guid? createdByUserGuid)
    {
        CreatedByUserGuid = createdByUserGuid;
        CreatedDate = DateTime.UtcNow;
    }

}
