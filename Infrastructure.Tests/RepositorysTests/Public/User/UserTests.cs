using FluentAssertions;

using Infrastructure.Tests.Common;
using Infrastructure.Tests.Common.Repositorys;
using Infrastructure.Tests.Common.Shared;

using SLAIS.Domain.Users;

using Xunit;

namespace Infrastructure.Tests.RepositorysTests.Public.User;

public class UserTests : TestBase
{
    private readonly UserTestRepository _userTestRepository;

    private readonly InstituteTestRepository _instituteTestRepository;

    public UserTests(PostgreSqlContainerFixture fixture)
        : base(fixture)
    {
        _userTestRepository = new UserTestRepository(fixture);
        _instituteTestRepository = new InstituteTestRepository(fixture);
    }

    #region Create

    [Fact]
    public async Task CreateAsync_ShouldPersistUser_WhenUserIsValid()
    {
        // Arrange
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = UserEntity
            .CreateAdmin(
            null,
            "test@test.com",
            "hashedpassword",
            "max_mustermann",
            "Max",
            "Mustermann",
            institute.Guid);

        await _userTestRepository.UserRepository
            .CreateAsync(user);
        await SaveChangesAsync();

        var persistedUser = await GetCreatedEntityByGuid<UserEntity>(user.Guid);

        Helpers.CheckCreatedUser(
            persistedUser,
            user,
            institute.Guid);
    }

    #endregion

    #region Get

    [Fact]
    public async Task GetUserByGuidAsync_ShouldReturnUser_WhenUserExists()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = await _userTestRepository
            .CreateUserAsync(
            institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByGuidAsync(user.Guid);

        Helpers.CheckCreatedUser(
            persistedUser,
            user,
            institute.Guid);
    }

    [Fact]
    public async Task GetUserByGuidAsync_ShouldReturnNull_WhenUserNotExists()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        await _userTestRepository
            .CreateUserAsync(
                institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByGuidAsync(Guid.CreateVersion7());

        persistedUser.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnUser_WhenEmailMatches()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = await _userTestRepository
            .CreateUserAsync(
            institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(user.Email);

        Helpers.CheckCreatedUser(
            persistedUser,
            user,
            institute.Guid);
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnUser_WhenUsernameMatches()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = await _userTestRepository
            .CreateUserAsync(
                institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(user.Username);

        Helpers.CheckCreatedUser(
            persistedUser,
            user,
            institute.Guid);
    }

    [Fact]
    public async Task GetUserByEmail_ShouldReturnNull_WhenEmailNotFound()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        await _userTestRepository
            .CreateUserAsync(
                institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync("test1@slais.de");

        persistedUser.Should().BeNull();
    }

    [Fact]
    public async Task GetUserByUsername_ShouldReturnNull_WhenUsernameNotFound()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        await _userTestRepository
            .CreateUserAsync(
                institute.Guid);

        var persistedUser = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync("userTest1234");

        persistedUser.Should().BeNull();
    }

    #endregion
}
