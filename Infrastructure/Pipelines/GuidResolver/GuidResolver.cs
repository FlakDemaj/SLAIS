using Application.Common;

using Domain.Common.Exceptions;

using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

using Microsoft.EntityFrameworkCore;

using SLAIS.Domain.Users;

namespace Infrastructure.Pipelines.GuidResolver;

public class GuidResolver
{
    private readonly SlaisDbContext _dbContext;

    public GuidResolver(SlaisDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Guid> ResolveAsync(
        int publicId,
        string entityType,
        CancellationToken cancellationToken)
    {
        var guidId = entityType switch
        {
            "User" => await _dbContext.GetNoTrackingSet<UserEntity>()
                .Where(p => p.Id == publicId)
                .Select(p => (Guid?)p.Guid)
                .FirstOrDefaultAsync(cancellationToken),

            _ => throw new SlaisException(CommonErrorCodes.DefaultErrorCode)
        };

        if (guidId is null)
        {
            throw new SlaisException(CommonErrorCodes.DefaultErrorCode);
        }

        return guidId.Value;
    }
}
