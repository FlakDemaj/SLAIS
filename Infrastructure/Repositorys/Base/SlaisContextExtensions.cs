using Application.Common;
using Application.Utils;

using Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using SLAIS.Domain.Commom;

namespace Infrastructure.Repositorys;

public static class SlaisContextExtensions
{
    public static async Task<TSaveType> CreateAsync<TSaveType>(
        this SlaisDbContext context,
        TSaveType saveData)
        where TSaveType : BaseGuidEntity
    {
        try
        {
            saveData.Guid = Guid.CreateVersion7();

            EntityEntry<TSaveType> result = context.Add(saveData);
            return result.Entity;
        }
        catch (SlaisException e)
        {
            throw new SlaisException(CommonErrorCodes.DatabaseError, e);
        }
    }

    public static IQueryable<T> GetNoTrackingSet<T>(this SlaisDbContext context)
        where T : class
    {
        return context.Set<T>().AsNoTracking();
    }

    public static async Task UpdateAndSaveChangesAsync(
        this SlaisDbContext context,
        object updateData)
    {
        try
        {
            context.Entry(updateData).State = EntityState.Modified;
        }
        catch (SlaisException)
        {
            throw;
        }
        catch (Exception exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                return;
            }

            throw new SlaisException(CommonErrorCodes.DatabaseError, exception);
        }
    }
}
