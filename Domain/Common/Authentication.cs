using Domain.Common.Enums;

namespace Domain.Common;

public class Authentication : IAuthentication
{
    public Guid UserGuid { get; private set; }
    public Roles UserRole { get; private set; }

    public Authentication(
        Guid userGuid,
        Roles userRole)
    {
        UserGuid = userGuid;
        UserRole = userRole;
    }
}
