using Domain.Common.Enums;

namespace Application.Common.Authentication;

public interface IAuthentication
{
    Guid UserGuid { get; }

    Guid InstitutionGuid { get; }

    Roles UserRole { get; }
}
