using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Users.Update;

public class UpdateProfileCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IStorageService storage,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _storage = storage;
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

        string profileUrl = user.ProfileUrl;

        if(command.ImageBytes is not null)
        {
            var uploadResult = await _storage.UploadProfileAsync(
                user.Id,
                command.ImageBytes,
                command.ImageContentType!,
                ct
            );

            profileUrl = uploadResult;
        }

        user.UpdateProfile(command.Name, profileUrl);

        await _unitOfWork.SaveChangesAsync(ct);

        return new UpdateProfileResult
        {
            Id = user.Id,
            Name = user.Name,
            ProfileUrl = user.ProfileUrl
        };
    }
}