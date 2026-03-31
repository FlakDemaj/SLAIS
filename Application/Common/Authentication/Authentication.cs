using Domain.Common.Enums;

namespace Application.Common.Authentication;

public class Authentication : IAuthentication
{
    public Guid UserGuid { get; }

    public Guid InstitutionGuid { get; }
    public Roles UserRole { get; }

    public Authentication(
        Guid userGuid,
        Roles userRole,
        Guid institutionGuid)
    {
        UserGuid = userGuid;
        UserRole = userRole;
        InstitutionGuid = institutionGuid;
    }
}
