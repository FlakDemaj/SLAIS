using System.ComponentModel;

namespace Application.Common;

public enum CommonErrorCodes
{
    [Description("Es gibt momentant Probleme mit dem Server.")]
    DefaultErrorCode = -100001,

    [Description("Es gibt momentant Probleme mit der Datenbank.")]
    DatabaseError = -100002
}
