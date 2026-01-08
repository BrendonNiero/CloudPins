namespace CloudPins.Application.Common.Excceptions;

public abstract class ApplicationException : Exception
{
    protected ApplicationException(string message)
    : base(message)
    {
        
    }
}