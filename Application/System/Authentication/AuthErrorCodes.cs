using System.ComponentModel;

namespace Application.Authentication;

public enum AuthErrorCodes
{
    [Description("Es wurde kein User mit dieser Email oder diesem Benutzername gefunden!")]
    NoUserWithThisName = -200001,

    [Description("Das angegebene Passwort ist falsch.")]
    WrongPassword = -200002,

    [Description("Ihr Konto ist gesperrt. Um das Konto zu aktivieren bitte das passwort zurücksetzen!")]
    UserIsBlocked = -200003
}
