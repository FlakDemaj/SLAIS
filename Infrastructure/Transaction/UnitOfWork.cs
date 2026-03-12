using Application.Utils;
using Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore.Storage;

namespace Infrastructure.Transaction;

public class UnitOfWork : IUnitOfWork
{
    private readonly SAISDbContext _context;

    public UnitOfWork(SAISDbContext context)
    {
        _context = context;
    }
    
    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _context.SaveChangesAsync(cancellationToken);
    }

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
       return _context.Database.BeginTransactionAsync(cancellationToken);
    }
}