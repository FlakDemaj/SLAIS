using Application.Common;
using Application.Common.Interfaces.Services;
using Application.Utils.Exceptions;
using Application.Utils.Logger;

using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

using SLAIS.Domain.Users;

namespace Infrastructure.Persistence.Seeding;

public class ServerUserSeeder
{
    private readonly SlaisDbContext _context;
    private readonly IPasswordHasher _passwordService;
    private readonly IConfiguration _configuration;
    private readonly ISlaisLogger<ServerUserSeeder> _logger;

    public ServerUserSeeder(
        SlaisDbContext context,
        IPasswordHasher passwordService,
        IConfiguration configuration,
        ISlaisLogger<ServerUserSeeder> logger)
    {
        _context = context;
        _passwordService = passwordService;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task SeedAsync()
    {
        var serverUser = await _context.GetNoTrackingSet<UserEntity>()
            .FirstOrDefaultAsync(user => user.Email == "noreply@slais.de");

        if (serverUser == null)
        {
            _logger.LogError("Server User is null", null);
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        if (serverUser.HashedPassword == "PLACEHOLDER")
        {
            var plainPassword = _configuration["Seeding:ServerUserPassword"];
            if (plainPassword == null)
            {
              _logger.LogError("Server User Password is empty", null);
              throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
            }

            var hashedPassword = _passwordService.Hash(plainPassword);
            serverUser.SetPassword(hashedPassword);

            await _context.SaveChangesAsync();
        }
    }
}
