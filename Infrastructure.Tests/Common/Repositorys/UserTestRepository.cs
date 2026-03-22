using System.Net;

using Domain.System.RefreshToken;

using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

using Microsoft.EntityFrameworkCore;

using SLAIS.Domain.Users;

namespace Infrastructure.Tests.Common.Repositorys;

public class UserTestRepository
{
    public readonly UserRepository UserRepository;

    private readonly SlaisDbContext _dbContext;

    public UserTestRepository(PostgreSqlContainerFixture fixture)
    {
        _dbContext = fixture.SlaisDbContext;
        UserRepository = new UserRepository(_dbContext);
    }

    public async Task<UserEntity> CreateUserAsync(
        Guid instituteGuid,
        Guid? createdByUserGuid = null,
        string email = "test@test.com",
        string firstname = "max",
        string lastname = "mustermann",
        string username = "max_mustermann69",
        string password = "testpassword")
    {
        var user = UserEntity.CreateAdmin(
            createdByUserGuid,
            email,
            password,
            username,
            firstname,
            lastname,
            instituteGuid);

        user = await UserRepository.CreateAsync(
            user);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task SaveChangesAsync()
    {
        await _dbContext.SaveChangesAsync();
    }

    public async Task<UserEntity> CreateRefreshTokenForUserAsync(
        UserEntity user,
        IPAddress? ipAddress = null,
        int expiresInDays = 7,
        Guid? deviceGuid = default,
        string deviceName = "test")
    {
        user.CreateRefreshToken(
            expiresInDays,
            deviceGuid ?? Guid.CreateVersion7(),
            deviceName,
            ipAddress ?? IPAddress.Loopback);

        await _dbContext.SaveChangesAsync();

        return user;
    }

    public async Task<UserEntity?> GetUserByGuid(Guid userGuid)
    {
        return await _dbContext.GetNoTrackingSet<UserEntity>()
            .FirstOrDefaultAsync(u => u.Guid == userGuid);
    }

    public async Task<RefreshTokenEntity?> GetRefreshTokenByUserGuid(Guid userGuid)
    {
        return await _dbContext
            .GetNoTrackingSet<RefreshTokenEntity>()
            .FirstOrDefaultAsync(rt => rt.UserGuid == userGuid);
    }
}
