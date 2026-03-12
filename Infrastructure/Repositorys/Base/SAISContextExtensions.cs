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
        finally
        {
            context.Entry(saveData).State = EntityState.Detached;
        }   
    }

    public static IQueryable<T> GetNoTrackingSet<T>(this SAISDbContext context)
        where T : class
    {
        return context.Set<T>().AsNoTracking();
    }
}