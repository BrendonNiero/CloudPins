namespace CloudPins.Application.Users.Update;

public class UpdateProfileCommand
{
    public string Name { get; init; } = string.Empty;
    public byte[]? ImageBytes { get; init; }
    public string? ImageFileName { get; init; }
    public string? ImageContentType { get; init; }
}