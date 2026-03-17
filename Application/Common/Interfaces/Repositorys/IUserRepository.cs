using SLAIS.Domain.Users;

namespace Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    Task<UserEntity?> GetUserByGuidAsync(Guid guid);

    Task<UserEntity?> GetUserByUsernameOrEmailAsync(string username);
}
