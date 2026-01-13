namespace CloudPins.Application.Users.Update;

public class UpdateProfileCommand
{
    public string Name { get; init; } = string.Empty;
    public string ProfileUrl { get; init; } = string.Empty;
}