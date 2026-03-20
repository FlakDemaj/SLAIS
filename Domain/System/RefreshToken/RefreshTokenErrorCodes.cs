using System.ComponentModel;

namespace Domain.Systems.RefreshToken;

public enum RefreshTokenErrorCodes
{
    [Description("Irgendwas lief falsch. Bitte versuchen Sie es bald erneut.")]
    InvalidExpiration = -400001,

    [Description("Irgendwas lief falsch. Bitte versuchen Sie es bald erneut.")]
    InvalidDeviceGuid = -400002,

    [Description("Irgendwas lief falsch. Bitte versuchen Sie es bald erneut.")]
    InvalidDeviceName = -400003,
}
