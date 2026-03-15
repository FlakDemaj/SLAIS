using Application.Interfaces;
using Infrastructure.Persistence.Context;
using SAIS.Domain.Commom;

namespace Infrastructure.Repositorys;

public class BaseRepository<T> : SAISRepository, IBaseRepository<T>
    where T : BaseGuidEntity
{
    protected BaseRepository(SAISDbContext context)
        : base(context)
    {
    }

    public Task<T> CreateAsync(T objectToCreate)
    {
        return _context.CreateAsync(objectToCreate);
    }

    public async Task UpdateAndSaveChangesAsync(T objectToUpdate)
    { 
       await _context.UpdateAndSaveChangesAsync(objectToUpdate);
    }
}