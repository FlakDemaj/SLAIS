using Domain.System.RefreshToken;

using FluentAssertions;

using Infrastructure.Tests.Common;
using Infrastructure.Tests.Common.Repositorys;

using SLAIS.Domain.Users;

using Xunit;

namespace Infrastructure.Tests.Public.User;

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

        // Act
        await _userTestRepository.UserRepository
            .CreateAsync(user);
        await _userTestRepository
            .SaveChangesAsync();

        // Assert
        var persistedUser = await _userTestRepository
            .GetUserByGuid(user.Guid);

        CheckCreatedUser(
            persistedUser,
            user,
            institute.Guid);
    }

    [Fact]
    public async Task CreateAsync_ShouldPersistUserWithRefreshToken_WhenUserIsValid()
    {
        var institute = await _instituteTestRepository
            .CreateInstituteAsync();

        var user = await _userTestRepository.CreateUserAsync(institute.Guid);

        await _userTestRepository
            .CreateRefreshTokenForUserAsync(user);

        var refreshToken = await _userTestRepository.GetRefreshTokenByUserGuid(user.Guid);

        CheckRefreshToken(
            refreshToken,
            user);
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

        CheckCreatedUser(
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

        CheckCreatedUser(
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

        CheckCreatedUser(
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

    [Fact]
    public async Task GetUserWithRefreshToken_ShouldReturnUser_WhenUserIsValid()
    {
        var institute = await _instituteTestRepository.CreateInstituteAsync();
        var user = await _userTestRepository.CreateUserAsync(institute.Guid);
        await _userTestRepository.CreateRefreshTokenForUserAsync(user);

        var userWithToken = await _userTestRepository
            .UserRepository
            .GetUserByUsernameOrEmailWithRefreshTokenAsync(user.Username);

        CheckCreatedUser(
            userWithToken,
            user,
            institute.Guid);

        user.RefreshTokens.Should().NotBeEmpty();
        user.RefreshTokens.ToList().Count.Should().Be(1);

        CheckRefreshToken(
            user.RefreshTokens.ToList()[0],
            user);
    }

    #endregion

    #region Helper

    private static void CheckCreatedUser(
        UserEntity? persistedUser,
        UserEntity user,
        Guid instituteGuid)
    {
        persistedUser.Should().NotBeNull();

        persistedUser.Guid.Should().Be(user.Guid);
        persistedUser.Email.Should().Be(user.Email);
        persistedUser.FirstName.Should().Be(user.FirstName);
        persistedUser.LastName.Should().Be(user.LastName);
        persistedUser.Username.Should().Be(user.Username);
        persistedUser.State.Should().Be(user.State);
        persistedUser.Role.Should().Be(user.Role);
        persistedUser.HashedPassword.Should().Be(user.HashedPassword);
        persistedUser.IsBlocked.Should().Be(user.IsBlocked);
        persistedUser.LoginAttempts.Should().Be(user.LoginAttempts);
        persistedUser.InstituteUuid.Should().Be(instituteGuid);

        persistedUser.CreatedDate.Should().BeCloseTo(
            user.CreatedDate,
            precision: TimeSpan.FromMicroseconds(1));
        persistedUser.CreatedByUserGuid.Should().Be(user.CreatedByUserGuid);

        persistedUser.DeleteDate.Should().Be(user.DeleteDate);
        persistedUser.DeletedByUserGuid.Should().Be(user.DeletedByUserGuid);

        persistedUser.UpdateDate.Should().Be(user.UpdateDate);
        persistedUser.UpdatedByUserGuid.Should().Be(user.UpdatedByUserGuid);
    }

    private static void CheckRefreshToken(
        RefreshTokenEntity? refreshToken,
        UserEntity user)
    {
        refreshToken.Should().NotBeNull();

        refreshToken.Guid.Should().NotBeEmpty();
        refreshToken.RefreshToken.Should().NotBeEmpty();
        refreshToken.CreatedDate.Should().BeAfter(user.CreatedDate);
        refreshToken.Revoked.Should().BeFalse();
        refreshToken.ExpirationDate.Should().BeCloseTo(DateTime.UtcNow.AddDays(7),
            precision: TimeSpan.FromSeconds(1));
        refreshToken.DeviceGuid.Should().NotBeEmpty();
        refreshToken.DeviceName.Should().NotBeEmpty();
        refreshToken.UserGuid.Should().Be(user.Guid);
        refreshToken.IpAddress.Should().NotBeNull();
        refreshToken.LastUsedDate.Should().BeBefore(DateTime.UtcNow);
        refreshToken.RevokedDate.Should().BeNull();
    }

    #endregion
}
