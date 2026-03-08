using SAIS.Domain.Users;

namespace SAIS.Domain.Commom;

public class CreatedByBaseEntity : BaseGuidEntity
{
    public Guid? CreatedByUserGuid { get; private set; }
    public DateTime CreatedDate { get; private set; }
    
    // Navigation property
    public UserEntity CreatedByUser { get; private set; }
}