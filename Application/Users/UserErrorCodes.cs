using System.ComponentModel;

namespace Application.Users;

public enum UserErrorCodes
{
    [Description("Ein User mit dieser Id wurde nicht gefunden!")]
    UserNotFound = -20001
}