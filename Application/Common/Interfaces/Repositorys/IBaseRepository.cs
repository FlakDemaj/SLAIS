using SAIS.Domain.Commom;

namespace Application.Interfaces;

public interface IBaseRepository<T>
    where T : BaseGuidEntity
{
    Task<T> CreateAsync(T objectToCreate);

    Task UpdateAndSaveChangesAsync(T objectToUpdate);
}
