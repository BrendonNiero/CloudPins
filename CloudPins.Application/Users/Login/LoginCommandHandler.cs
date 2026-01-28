using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Users.Login;

public class LoginCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;

    public LoginCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
    }

    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken ct)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email, ct);

        if(user is null)
            throw new UnauthorizedException("Invalid credentials.");

        var validPassword = user.CheckPassword(
            command.Password,
            _passwordHasher.Verify
        );

        if(!validPassword)
            throw new UnauthorizedException("Invalid credentials.");


        return new LoginResult(
            user.Id,
            user.Name,
            user.Email,
            user.ProfileUrl
        );
    }
}