using Domain.Common;
using Domain.Common.Enums;

namespace Application.Common.Authentication;

public class Authentication : IAuthentication
{
    public Guid UserGuid { get; private set; }

    public Guid InstitutionGuid { get; private set; }
    public Roles UserRole { get; private set; }

    public Authentication(
        Guid userGuid,
        Roles userRole,
        Guid institutionGuid)
    {
        UserGuid = userGuid;
        UserRole = userRole;
        InstitutionGuid  = institutionGuid;
    }
}
