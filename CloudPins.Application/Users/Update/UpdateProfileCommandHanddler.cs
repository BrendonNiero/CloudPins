using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Common.Storage;

namespace CloudPins.Application.Users.Update;

public class UpdateProfileCommandHandler
{
    private readonly IUserRepository _userRepository;
    private readonly IUserReadRepository _userReadRepository;
    private readonly IStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateProfileCommandHandler(
        IUserRepository userRepository,
        IUserReadRepository userReadRepository,
        IStorageService storage,
        IUnitOfWork unitOfWork
    )
    {
        _userRepository = userRepository;
        _userReadRepository = userReadRepository;
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
            var key = StorageKeyBuilder.BuilderProfileImageKey(
                user.Id,
                command.ImageFileName!
            );

            var uploadResult = await _storage.UploadAsync(
                key,
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