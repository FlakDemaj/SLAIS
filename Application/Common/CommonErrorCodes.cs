using System.ComponentModel;

namespace Application.Common;

public enum CommonErrorCodes
{
    [Description("Es gibt momentant Probleme mit dem Server.")]
    DefaultErrorCode = -00001,
    
    [Description("Es gibt momentant Probleme mit der Datenbank.")]
    DatabaseError = -00002
}