namespace Application.Utils;

public class SAISException : Exception
{
    public int ErrorCode { get; }
    
    public string ErrorMessage { get; }

    public SAISException(Enum errorCode) :
        base(errorCode.GetDescription())
    {
        ErrorCode = Convert.ToInt32(errorCode);
        ErrorMessage = errorCode.GetDescription();
    }

    public SAISException(Enum errorCode, Exception exception) :
        base(errorCode.GetDescription(), exception)
    {
        ErrorCode = Convert.ToInt32(errorCode);
        ErrorMessage = errorCode.GetDescription();
    }
    
    
}