using System.ComponentModel;

namespace Application.Authentication;

public enum AuthErrorCodes
{
    [Description("Es wurde kein User mit dieser Email oder diesem Benutzername gefunden!")]
    NoUserWithThisName = -200001,

    [Description("Das angegebene Passwort ist falsch.")]
    WrongPassword = -200002,

    [Description("Ex existiert kein Nutzer mit diesem Token. Bitte melden Sie sich beim Support!")]
    NoUserWithThisToken = -200003,

    [Description("Ihre Session ist abgelaufen. Bitte melden Sie sich wieder an.")]
    NoValidTokenFound = -200004,
}
