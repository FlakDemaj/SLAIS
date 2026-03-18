using SLAIS.Domain.Commom;
using SLAIS.Domain.Users;

namespace Domain.Systems.RefreshToken;

public abstract class RefreshTokenNavigationPropertyEntity : BaseGuidEntity
{
    //Navigation Property for user
    public UserEntity User { get; private set; }

}
