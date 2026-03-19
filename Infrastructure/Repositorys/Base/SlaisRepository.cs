using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositorys;

public class SlaisRepository
{
    protected readonly SlaisDbContext _context;

    protected SlaisRepository(SlaisDbContext context)
    {
        _context = context;
    }
}
