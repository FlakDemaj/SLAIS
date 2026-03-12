using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositorys;

public class SAISRepository
{
    protected readonly SAISDbContext _context;

    protected SAISRepository(SAISDbContext context)
    {
        _context = context;
    }
}