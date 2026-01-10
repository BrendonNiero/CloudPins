namespace CloudPins.Application.Common.Exceptions;

public sealed class ForbiddenException : ApplicationException
{
    public ForbiddenException(string message)
    : base(message)
    {
        
    }
}