using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Users.Update;

public class UpdateProfileCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IUserReadRepository userReadRepository,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _userReadRepository = userReadRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<UpdateProfileResult> Handle(
        UpdateProfileCommand command,
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var user = await _userRepository.GetByIdAsync(currentUserId, ct);
        if(user is null)
            throw new NotFoundException("User not found.");
    }
}