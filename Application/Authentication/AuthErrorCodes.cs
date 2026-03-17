using System.ComponentModel;

namespace Application.Authentication.Commands.Login;

public enum AuthErrorCodes
{
    [Description("Es wurde kein User mit dieser Email oder diesem Benutzername gefunden!")]
    NoUserWithThisName = -20001,

    [Description("Das angegebene Passwort ist falsch.")]
    WrongPassword = -20002,

    [Description("Ihr Konto ist gesperrt. Um das Konto zu aktivieren bitte das passwort zurücksetzen!")]
    UserIsBlocked = -20003,
}
