using CloudPins.Domain.Common;

namespace CloudPins.Domain.Users;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string ProfileUrl { get; private set; } = string.Empty;

    protected User() {}

    public User(string name, string email, string profileUrl)
    {
        Id = Guid.NewGuid();
        Name = name;
        Email = email;
        ProfileUrl = profileUrl;
        CreatedAt = DateTime.UtcNow;
    }

    public void UpdateProfile(string name, string profileUrl)
    {
        Name = name;
        ProfileUrl = profileUrl;
    }
}