using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Users;

namespace CloudPins.Application.Users.Create;

public class CreateUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken ct)
    {
        var exists = await _userRepository.ExistsByEmailAsync(command.Email, ct);
        if(exists)
            throw new ConflictException("Email already registred.");

        var passwordHash = _passwordHasher.Hash(command.Passowrd);

        var user = User.Create(
            command.Name,
            command.ProfileUrl,
            command.Email,
            passwordHash
        );

        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync();

        return new CreateUserResult(
            user.Id,
            user.Name,
            user.Email,
            user.ProfileUrl
        );
    }
}