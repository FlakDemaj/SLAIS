using SAIS.Domain.Users;

namespace SAIS.Domain.Commom;

public class BaseDeleteByEntity : BaseUpdatedByEntity
{
    public Guid? DeletedByUserGuid { get; private set; }
    public DateTime? DeleteDate { get; private set; }

    // Navigation property
    public UserEntity? DeletedByUser { get; private set; }
}
