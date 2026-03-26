using FluentAssertions;

using Infrastructure.Tests.Common;
using Infrastructure.Tests.Common.Repositorys;
using Infrastructure.Tests.Common.Shared;

using Xunit;

namespace Infrastructure.Tests.RepositoryTests.Public.User;

public class UserRefreshTokenTests : TestBase
{
    private readonly UserTestRepository _userTestRepository;

    private readonly InstituteTestRepository _instituteTestRepository;

    public UserRefreshTokenTests(PostgreSqlContainerFixture fixture)
        : base(fixture)
    {
        _userTestRepository = new UserTestRepository(fixture);
        _instituteTestRepository = new InstituteTestRepository(fixture);
    }

    #region Create

    [Fact]
    public async Task CreateAsync_ShouldPersistUserWithRefreshToken_WhenUserIsValid()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = await _userTestRepository.CreateUserAsync(institute.Guid);

        await _userTestRepository
            .CreateRefreshTokenForUserAsync(user);

        var refreshToken = await _userTestRepository.GetRefreshTokenByUserGuidAsync(user.Guid);

        Helpers.CheckCreatedRefreshToken(
            refreshToken,
            user);
    }

    #endregion

    #region Get

    [Fact]
    public async Task GetUserWithRefreshToken_ShouldReturnUser_WhenUserIsValid()
    {
        var institute = await _instituteTestRepository.CreateInstituteAsync();
        var user = await _userTestRepository.CreateUserAsync(institute.Guid);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);

        var userWithToken = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(user.Username);

        Helpers.CheckCreatedUser(
            userWithToken,
            user,
            institute.Guid);

        user.RefreshTokens.Should().NotBeEmpty();
        user.RefreshTokens.ToList().Count.Should().Be(1);

        Helpers.CheckCreatedRefreshToken(
            user.RefreshTokens.ToList()[0],
            user);
    }

    [Fact]
    public async Task GetUserWithRefreshToken_ShouldReturnNoUser_WhenUsernameIsFalse()
    {
        var institute = await _instituteTestRepository.CreateInstituteAsync();
        var user = await _userTestRepository.CreateUserAsync(institute.Guid);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);

        var userWithToken = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync("User");

        userWithToken.Should().BeNull();
    }

    [Fact]
    public async Task GetUserWithRefreshTokenByGuid_ShouldReturnUser_WhenUserIsValid()
    {
        var institute = await _instituteTestRepository.CreateInstituteAsync();
        var user = await _userTestRepository.CreateUserAsync(institute.Guid);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);

        var refreshTokens = user.RefreshTokens.ToList();

        var userWithTokens = await _userTestRepository
            .UserRepository
            .GetUserWithRefreshTokensByGuidAsync(refreshTokens[1].RefreshToken);

        userWithTokens.Should().NotBeNull();
        userWithTokens.RefreshTokens.Count.Should().Be(2);

        Helpers.CheckCreatedUser(
            userWithTokens,
            user,
            institute.Guid);

        Helpers.CheckCreatedRefreshToken(refreshTokens[0], user);
        Helpers.CheckCreatedRefreshToken(refreshTokens[1], user);
    }

    [Fact]
    public async Task GetUserWithRefreshTokenByGuid_ShouldReturnNoTokens_WhenTokenGuidIsNotValid()
    {
        var institute = await _instituteTestRepository.CreateInstituteAsync();
        var user = await _userTestRepository.CreateUserAsync(institute.Guid);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);

        var userWithTokens = await _userTestRepository
            .UserRepository
            .GetUserWithRefreshTokensByGuidAsync(Guid.CreateVersion7());

        userWithTokens.Should().BeNull();
    }

    #endregion
}
