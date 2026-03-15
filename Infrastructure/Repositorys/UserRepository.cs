using Application.Interfaces;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SAIS.Domain.Users;

namespace Infrastructure.Repositorys;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(SAISDbContext context)
        : base(context)
    {
    }

    public Task<UserEntity?> GetUserByGuidAsync(Guid userGuid)
    {
        return _context
               .GetNoTrackingSet<UserEntity>()
               .FirstOrDefaultAsync(user => user.Guid == userGuid);
    }

    public Task<UserEntity?> GetUserByUsernameOrEmailAsync(string username)
    {
        return _context
            .GetNoTrackingSet<UserEntity>()
            .FirstOrDefaultAsync(user => user.Email == username 
                                         || user.Username == username);
    }
}