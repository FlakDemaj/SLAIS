using SAIS.Domain.Users;

namespace Application.Interfaces;

public interface IUserRepository : IBaseRepository<UserEntity>
{
    public Task<UserEntity?> GetUserByGuidAsync(Guid guid);
    
    public Task<UserEntity?> GetUserByUsernameOrEmailAsync(string username);
}