using Domain.Institutes;

using Infrastructure.Persistence.Context;
using Infrastructure.Repositorys;

namespace Infrastructure.Tests.Common.Repositorys;

public class InstituteTestRepository
{
    private readonly InstituteRepository _instituteRepository;
    private readonly SlaisDbContext _dbContext;

    public InstituteTestRepository(PostgreSqlContainerFixture fixture)
    {
        _dbContext = fixture.SlaisDbContext;
        _instituteRepository = new InstituteRepository(_dbContext);
    }

    public async Task<InstituteEntity> CreateInstituteAsync(
        Guid? createdByUserGuid = null,
        string name = "TestInstitute",
        string branch = "Health")
    {
        var institute = InstituteEntity.Create(
            createdByUserGuid,
            name,
            branch);

        institute = await _instituteRepository.CreateAsync(institute);
        await _dbContext.SaveChangesAsync();

        return institute;
    }
}

