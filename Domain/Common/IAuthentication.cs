using Domain.Common.Enums;

namespace Domain.Common;

public interface IAuthentication
{
    Guid UserGuid { get; }

    Roles UserRole { get; }
}
