namespace CloudPins.Application.Users.Create;

public record CreateUserResult(
    Guid Id,
    string Name,
    string Email,
    string ProfileUrl,
    string token
);