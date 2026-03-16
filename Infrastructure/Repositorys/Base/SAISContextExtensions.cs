using Application.Common;
using Application.Utils;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using SAIS.Domain.Commom;

namespace Infrastructure.Repositorys;

public static class SAISContextExtensions
{
    public static async Task<TSaveType> CreateAsync<TSaveType>(
        this SAISDbContext context,
        TSaveType saveData)
        where TSaveType : BaseGuidEntity
    {
        try
        {
            saveData.Guid = Guid.CreateVersion7();

            var result = context.Add(saveData);
            return result.Entity;
        }
        catch (SAISException e)
        {
            throw new SAISException(CommonErrorCodes.DatabaseError, e);
        }
    }

    public static IQueryable<T> GetNoTrackingSet<T>(this SAISDbContext context)
        where T : class
    {
        return context.Set<T>().AsNoTracking();
    }

    public static async Task UpdateAndSaveChangesAsync(
        this SAISDbContext context,
        object updateData)
    {
        try
        {
            context.Entry(updateData).State = EntityState.Modified;
        }
        catch (SAISException)
        {
            throw;
        }
        catch (Exception exception)
        {
            if (exception is DbUpdateConcurrencyException)
            {
                return;
            }

            throw new SAISException(CommonErrorCodes.DatabaseError, exception);
        }
    }
}