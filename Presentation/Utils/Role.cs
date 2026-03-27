using Domain.Common.Enums;

namespace Presentation.Utils;

public class Role
{
    internal static class RoleConstants
    {
        internal const string Server = nameof(Roles.Server);

        internal const string Admin = nameof(Roles.Admin);

        internal const string Teacher = nameof(Roles.Teacher);

        internal const string Student = nameof(Roles.Student);
    }
}
