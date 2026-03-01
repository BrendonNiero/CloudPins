using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Users;

namespace CloudPins.Application.Users.Create;

public class CreateUserCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IStorageService _storage;
    private readonly IJwtTokenGenerator _jwt;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(
        IUserRepository userRepository,
        IPasswordHasher passwordHasher,
        IStorageService storage,
        IJwtTokenGenerator jwt,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _storage = storage;
        _jwt = jwt;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken ct)
    {
        var exists = await _userRepository.ExistsByEmailAsync(command.Email, ct);
        if(exists)
            throw new ConflictException("Email already registred.");

        if(command.ImageBytes is null)
            throw new Exception("No image uploaded.");

        var uploadResult = await _storage.UploadAsync(
            command.ImageBytes,
            command.ImageContentType!,
            ct
        );

        var passwordHash = _passwordHasher.Hash(command.Password);

        var user = User.Create(
            command.Name,
            uploadResult,
            command.Email,
            passwordHash
        );

        await _userRepository.AddAsync(user, ct);
        await _unitOfWork.SaveChangesAsync();

        var token =  _jwt.Generate(user.Id, user.Email);

        return new CreateUserResult(
            user.Id,
            user.Name,
            user.Email,
            user.ProfileUrl,
            token
        );
    }
}