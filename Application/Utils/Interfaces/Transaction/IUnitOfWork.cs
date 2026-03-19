using Infrastructure.Transaction;

namespace Application.Utils.Interfaces.Transaction;

public interface IUnitOfWork
{
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
}
