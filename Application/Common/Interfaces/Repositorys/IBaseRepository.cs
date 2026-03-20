using SLAIS.Domain.Commom;

namespace Application.Interfaces;

public interface IBaseRepository<T>
    where T : BaseGuidEntity
{
    Task<T> CreateAsync(T objectToCreate);
}
