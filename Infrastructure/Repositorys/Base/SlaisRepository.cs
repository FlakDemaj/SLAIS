using Infrastructure.Persistence.Context;

namespace Infrastructure.Repositorys;

public class SlaisRepository
{
    protected readonly SlaisDbContext Context;

    protected SlaisRepository(SlaisDbContext context)
    {
        Context = context;
    }
}
