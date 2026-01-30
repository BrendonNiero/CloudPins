namespace CloudPins.Application.Users.Login;

public record LoginResult(
    Guid UserId,
    string Name, 
    string Email,
    string ProfileUrl,
    string token
);