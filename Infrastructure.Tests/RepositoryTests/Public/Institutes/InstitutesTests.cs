using Domain.Institutes;

using Infrastructure.Tests.Common;
using Infrastructure.Tests.Common.Repositorys;
using Infrastructure.Tests.Common.Shared;

using Xunit;

namespace Infrastructure.Tests.RepositoryTests.Public.Institutes;

public class InstitutesTests : TestBase
{
    private readonly UserTestRepository _userTestRepository;

    private readonly InstituteTestRepository _instituteTestRepository;

    public InstitutesTests(PostgreSqlContainerFixture fixture)
        : base(fixture)
    {
        _userTestRepository = new UserTestRepository(fixture);
        _instituteTestRepository = new InstituteTestRepository(fixture);
    }

    #region Create

    [Fact]
    public async Task CreateInstitute_ShouldCreateInstitute_WithoutUser()
    {
        var institute = InstituteEntity.Create(
            null, "test", "Health");

        await _instituteTestRepository.InstituteRepository.CreateAsync(institute);
        await SaveChangesAsync();

        var createdInstitute = await GetCreatedEntityByGuid<InstituteEntity>(institute.Guid);

        Helpers.CheckCreatedInstitute(createdInstitute, institute);
    }

    [Fact]
    public async Task CreateInstitute_ShouldCreateInstitute_WithUser()
    {
        var firstInstitute = await _instituteTestRepository.CreateInstituteAsync();

        var user = await _userTestRepository.CreateUserAsync(firstInstitute.Guid);

        var institute = InstituteEntity.Create(
            user.Guid, "test", "Health");

        await _instituteTestRepository.InstituteRepository.CreateAsync(institute);
        await SaveChangesAsync();

        var createdInstitute = await GetCreatedEntityByGuid<InstituteEntity>(institute.Guid);

        Helpers.CheckCreatedInstitute(createdInstitute, institute);
    }

    #endregion
}
