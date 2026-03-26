using Domain.Institutes;

using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

namespace Infrastructure.Tests.Common.Repositorys;

public class InstituteTestRepository
{
    public readonly InstituteRepository InstituteRepository;

    private readonly SlaisDbContext _dbContext;

    public InstituteTestRepository(PostgreSqlContainerFixture fixture)
    {
        _dbContext = fixture.SlaisDbContext;
        InstituteRepository = new InstituteRepository(_dbContext);
    }

    public async Task<InstituteEntity> CreateInstituteAsync(
        Guid? createdByUserGuid = null,
        string? name = null,
        string? branch = null)
    {
        var institute = InstituteEntity.Create(
            createdByUserGuid,
            name ?? "TestInstitute",
            branch ?? "Health");

        institute = await InstituteRepository.CreateAsync(institute);
        await _dbContext.SaveChangesAsync();

        return institute;
    }
}
