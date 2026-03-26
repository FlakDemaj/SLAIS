using Domain.Common.Enums;

namespace Presentation.Utils;

public class Role
{
    public static class RoleConstants
    {
        public const string Server = nameof(Roles.Server);

        public const string Admin = nameof(Roles.Admin);

        public const string Teacher = nameof(Roles.Teacher);

        public const string Student = nameof(Roles.Student);

    }
}
