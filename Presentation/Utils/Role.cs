using Domain.Common.Enums;

namespace Presentation.Utils;

public class Role
{
    protected static class RoleConstants
    {
        protected const string Server = nameof(Roles.Server);

        protected const string Admin = nameof(Roles.Admin);

        protected const string Teacher = nameof(Roles.Teacher);

        protected const string Student = nameof(Roles.Student);

    }
}
