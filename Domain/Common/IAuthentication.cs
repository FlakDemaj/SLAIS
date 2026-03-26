using Domain.Common.Enums;

namespace Domain.Common;

public interface IAuthentication
{
    public Guid UserGuid { get; }

    public Roles UserRole { get; }
}
