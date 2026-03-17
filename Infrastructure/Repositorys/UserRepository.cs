using Application.Interfaces;

using Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

using SAIS.Domain.Users;

namespace Infrastructure.Repositorys;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(SlaisDbContext context)
        : base(context)
    {
    }

    public Task<UserEntity?> GetUserByGuidAsync(Guid userGuid)
    {
        return Context
               .GetNoTrackingSet<UserEntity>()
               .FirstOrDefaultAsync(user => user.Guid == userGuid);
    }

    public Task<UserEntity?> GetUserByUsernameOrEmailAsync(string username)
    {
        return Context
            .GetNoTrackingSet<UserEntity>()
            .FirstOrDefaultAsync(user => user.Email == username
                                         || user.Username == username);
    }
}
