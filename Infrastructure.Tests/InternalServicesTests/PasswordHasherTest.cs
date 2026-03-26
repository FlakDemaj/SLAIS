using FluentAssertions;

using Infrastructure.InternalServices;

using Xunit;

namespace Infrastructure.Tests;

public class PasswordHasherTest
{
    private readonly PasswordHasher _passwordHasher;

    public PasswordHasherTest()
    {
        _passwordHasher = new PasswordHasher();
    }

    [Fact]
    public void PasswordHasher_HashPassword()
    {
        var password = "password";
        var hashedPassword = _passwordHasher.Hash(password);

        hashedPassword.Should().NotBe(password);
    }

    [Fact]
    public void PasswordHasher_VerifyHashedPassword_ReturnTrue()
    {
        var password = "password";
        var hashedPassword = _passwordHasher.Hash(password);

        var isTheSame = _passwordHasher.Verify("password", hashedPassword);

        isTheSame.Should().BeTrue();
    }

    [Fact]
    public void PasswordHasher_VerifyHashedPassword_ReturnFalse()
    {
        var password = "password";
        var hashedPassword = _passwordHasher.Hash(password);

        var isTheSame = _passwordHasher.Verify("password!", hashedPassword);

        isTheSame.Should().BeFalse();
    }
}
