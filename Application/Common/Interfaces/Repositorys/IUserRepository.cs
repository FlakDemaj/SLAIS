using Domain.Common.Enums;

using SLAIS.Domain.Users;

namespace Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetUserByGuidAsync(Guid userGuid, Guid instituteGuid);

    Task<List<UserEntity>> GetAllUsersFromInstitute(
        Guid instituteGuid,
        Roles userRole);

    Task<UserEntity?> GetUserByUsernameOrEmailWithRefreshTokenAsync(string username);

    Task<UserEntity?> GetUserWithRefreshTokensByGuidAsync(Guid refreshTokenGuid);
}
