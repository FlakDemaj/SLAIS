using SAIS.Domain.Users;

namespace SAIS.Domain.Commom;

public class BaseDeleteByEntity : BaseUpdatedByEntity
{
    public Guid? DeleteByUserGuid { get; private set; }
    public DateTime? DeleteDate { get; private set; }
    
    // Navigation property
    public UserEntity? DeleteByUser { get; private set; }
}