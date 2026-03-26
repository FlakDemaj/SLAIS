using Domain.Base;

using SLAIS.Domain.Users;

namespace SLAIS.Domain.Commom;

public abstract class BaseUpdatedByEntity : BaseCreatedByEntity
{
    public Guid? UpdatedByUserGuid { get; private set; }

    public DateTime? UpdateDate { get; private set; }

    // Navigation property
    public UserEntity? UpdatedByUser { get; private set; }

    protected BaseUpdatedByEntity(
        Guid? createdByUserGuid)
        : base(createdByUserGuid)
    {
        UpdatedByUserGuid = null;
        UpdateDate = null;
    }

}
