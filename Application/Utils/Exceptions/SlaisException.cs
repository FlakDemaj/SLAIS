namespace Application.Utils.Exceptions;

public class SlaisException : Exception
{
    public int ErrorCode { get; }

    public string ErrorMessage { get; }

    public SlaisException(Enum errorCode) :
        base(errorCode.GetDescription())
    {
        ErrorCode = Convert.ToInt32(errorCode);
        ErrorMessage = errorCode.GetDescription();
    }

    public SlaisException(Enum errorCode, System.Exception exception) :
        base(errorCode.GetDescription(), exception)
    {
        ErrorCode = Convert.ToInt32(errorCode);
        ErrorMessage = errorCode.GetDescription();
    }


}
