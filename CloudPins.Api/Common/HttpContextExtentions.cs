using CloudPins.Application.Common.Exceptions;

namespace CloudPins.Api.Common;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var id = context.User.FindFirst("userId")?.Value;

        if(string.IsNullOrWhiteSpace(id))
        {
            throw new UnauthorizedException("UserId claim not found.");
        }

        return Guid.Parse(id);
    }
}