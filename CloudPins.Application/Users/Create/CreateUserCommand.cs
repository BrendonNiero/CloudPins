namespace CloudPins.Application.Users.Create;

public record CreateUserCommand(
    string Name,
    string Email,
    string Passowrd,
    string ProfileUrl
);