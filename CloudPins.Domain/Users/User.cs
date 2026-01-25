using CloudPins.Domain.Common;

namespace CloudPins.Domain.Users;

public class User : BaseEntity
{
    public string Name { get; private set; } = string.Empty;
    public string ProfileUrl { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;
    public string PasswordHash { get; private set; } = string.Empty;

    protected User() {}

    public static User Create(string name, string profileUrl, string email, string passwordHash)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");

        if(string.IsNullOrWhiteSpace(email))
            throw new ArgumentException("Email is required");

        if(string.IsNullOrWhiteSpace(profileUrl))
            throw new ArgumentException("Profile image is required");

        if(string.IsNullOrWhiteSpace(passwordHash))
            throw new ArgumentException("Password is required");

        return new User
        {
            Id = Guid.NewGuid(),
            Name = name,
            ProfileUrl = profileUrl,
            Email = email.ToLowerInvariant(),
            PasswordHash = passwordHash,
            CreatedAt = DateTime.UtcNow
        };
    }

    public bool CheckPassword(string password, Func<string, string, bool> verify)
    {
        return verify(password, PasswordHash);
    }

    public void UpdateProfile(string name, string profileUrl)
    {
        Name = name;
        ProfileUrl = profileUrl;
    }
}