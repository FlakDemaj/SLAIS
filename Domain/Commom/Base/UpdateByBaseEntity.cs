using SAIS.Domain.Users;

namespace SAIS.Domain.Commom;

public class UpdateByBaseEntity : CreatedByBaseEntity
{
    public Guid? UpdateByUserGuid { get; private set; }
    public DateTime? UpdateDate { get; private set; }
    
    // Navigation property
    public UserEntity? UpdateByUser { get; private set; }
}