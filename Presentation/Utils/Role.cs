using Domain.Common.Enums;

namespace Presentation.Utils;

public static class Role
{
    public const string Server = nameof(Roles.Server);

    public const string Admin = nameof(Roles.Admin);

    public const string SuperAdmin = nameof(Roles.Admin);

    public const string Teacher = nameof(Roles.Teacher);

    public const string Student = nameof(Roles.Student);
}
