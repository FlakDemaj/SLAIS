using SLAIS.Domain.Users;

namespace Domain.Tests.Public.Users;

public abstract class UserTestBase
{
    protected readonly UserEntity _user;

    protected UserTestBase()
    {
        _user = UserEntity.CreateAdmin(
            Guid.CreateVersion7(),
            "test@slais.de",
            "HashedPassword",
            "testAdmin",
            "Max",
            "Mustermann",
            Guid.CreateVersion7());
    }
}
