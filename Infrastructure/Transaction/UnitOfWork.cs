using Application.Utils.Interfaces.Transaction;

using Infrastructure.Persistence.Context;

using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Transaction;

public class UnitOfWork : IUnitOfWork
{
    private readonly SlaisDbContext _context;

    public UnitOfWork(SlaisDbContext context)
    {
        _context = context;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        return new EfTransaction(transaction);
    }
}
