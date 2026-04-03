using System.ComponentModel;

namespace Application.Public.Users;

public enum UserErrorCodes
{
    [Description("An user with this Id was not found.")]
    UserNotFound = -300001,

    [Description("Sie haben keinen Zugriff auf diese Funktion.")]
    Forbidden = -300003
}
