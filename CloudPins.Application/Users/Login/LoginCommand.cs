namespace CloudPins.Application.Users.Login;

public record LoginCommand(
    string Email,
    string Password
);