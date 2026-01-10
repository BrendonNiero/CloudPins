namespace CloudPins.Application.Common.Exceptions;

public sealed class UnauthorizedException : ApplicationException
{
    public UnauthorizedException(string message)
    : base(message)
    {
        
    }
}