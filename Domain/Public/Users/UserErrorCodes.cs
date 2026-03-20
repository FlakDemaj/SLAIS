using System.ComponentModel;

namespace Domain.Public.Users;

public enum UserErrorCodes
{
    [Description("Der Nutzer ist schon blockiert.")]
    UserIsAlreadyBlocked = -310001,

    [Description("Das Password ist nicht erlaubt.")]
    InvalidPassword = -310002,

    [Description("Leider sind die angegebenen Daten nicht erlaubt.")]
    InvalidInput = -310003
}
