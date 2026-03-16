using Domain.Systems.RefreshToken;

namespace Application.Interfaces;

public interface IRefreshTokenRepository : IBaseRepository<RefreshTokenEntity>
{
    public Task<RefreshTokenEntity> CreateAsync(RefreshTokenEntity objectToCreate);

    public Task UpdateAndSaveChangesAsync(RefreshTokenEntity objectToUpdate);
}