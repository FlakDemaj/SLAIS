using SLAIS.Domain.Users;

namespace SLAIS.Domain.Commom;

public class BaseCreatedByEntity : BaseGuidEntity
{
    public Guid? CreatedByUserGuid { get; private set; }
    public DateTime CreatedDate { get; private set; }

    // Navigation property
    public UserEntity? CreatedByUser { get; private set; }
}
