using Application.Interfaces;
using Domain.Systems.RefreshToken;
using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositorys;

public class RefreshTokenRepository : BaseRepository<RefreshTokenEntity>, IRefreshTokenRepository
{
    public RefreshTokenRepository(SAISDbContext context) : base(context)
    {
    }
}