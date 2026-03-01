namespace CloudPins.Application.Users.Create;
public class CreateUserCommand
{
    public string Name { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;
    public byte[]? ImageBytes { get; init; }
    public string? ImageFileName { get; init; }
    public string? ImageContentType { get; init; }
}