using Application.Interfaces;

using Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;

using SLAIS.Domain.Users;

namespace Infrastructure.Repositorys;

public class UserRepository : BaseRepository<UserEntity>, IUserRepository
{
    public UserRepository(SlaisDbContext context)
        : base(context)
    {
    }

    public Task<UserEntity?> GetUserByGuidAsync(Guid userGuid)
    {
        return _context
               .GetTrackingSet<UserEntity>()
               .FirstOrDefaultAsync(user => user.Guid == userGuid);
    }

    public Task<UserEntity?> GetUserByUsernameOrEmailWithRefreshTokenAsync(string username)
    {
        return _context
            .GetTrackingSet<UserEntity>()
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user => user.Email == username
                                         || user.Username == username);
    }

    public async Task<UserEntity?> GetUserWithRefreshTokensByGuidAsync(Guid refreshTokenGuid)
    {
        return await _context
            .GetTrackingSet<UserEntity>()
            .Include(user => user.RefreshTokens)
            .FirstOrDefaultAsync(user
                => user.RefreshTokens.Any(rt
                    => rt.RefreshToken == refreshTokenGuid
                ));
    }
}
