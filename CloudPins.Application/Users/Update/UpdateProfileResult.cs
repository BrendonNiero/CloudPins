namespace CloudPins.Application.Users.Update;

public class UpdateProfileResult
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string ProfileUrl { get; init; } = string.Empty;
}