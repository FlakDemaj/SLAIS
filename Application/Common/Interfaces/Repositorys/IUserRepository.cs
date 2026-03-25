using Domain.System.RefreshToken;

using SLAIS.Domain.Users;

namespace Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetUserByGuidAsync(Guid userGuid);

    Task<UserEntity?> GetUserByUsernameOrEmailWithRefreshTokenAsync(string username);

    Task<UserEntity?> GetUserWithRefreshTokensByGuidAsync(Guid refreshTokenGuid);
}
