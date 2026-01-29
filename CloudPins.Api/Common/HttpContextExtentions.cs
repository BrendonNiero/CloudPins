namespace CloudPins.Api.Common;

public static class HttpContextExtensions
{
    public static Guid GetUserId(this HttpContext context)
    {
        var id = context.User.FindFirst("userId")?.Value;
        return Guid.Parse(id!);
    }
}