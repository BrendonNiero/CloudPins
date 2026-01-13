namespace CloudPins.Application.Users.Get;

public record UserProfileDto(
    Guid Id,
    string Name,
    string Email,
    string ProfileUrl
);