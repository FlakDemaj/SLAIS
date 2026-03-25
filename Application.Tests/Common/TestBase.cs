using System.Net;

using AutoMapper;

using Domain.Institutes;

using SLAIS.Domain.Users;

namespace Application.Tests.Common;

public class TestBase
{

    protected static UserEntity BuildValidUser()
    {
        var institute = InstituteEntity.Create(
            createdByUserGuid: null,
            name: "testInstitute",
            branch: "Health");

        return UserEntity.CreateAdmin(
            instituteGuid: institute.Guid,
            createdByUserGuid: null,
            username: "testuser",
            email: "test@test.com",
            hashedPassword: "hashed-password",
            firstName: "Max",
            lastName: "Mustermann");
    }

    protected static UserEntity BuildValidUserWithRefreshTokens(short numberOfRefreshTokens)
    {
        var user = BuildValidUser();

        for (short i = 0; i < numberOfRefreshTokens; i++)
        {
            user.CreateRefreshToken(
                30,
                Guid.CreateVersion7(),
                $"Iphone I{i}",
                IPAddress.Loopback);
        }

        return user;
    }
}
