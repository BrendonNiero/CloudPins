using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Users.Get;

public class GetProfileQueryHandler
{
    private readonly IUserReadRepository _userReadRepository;

    public GetProfileQueryHandler(IUserReadRepository userReadRepository)
    {
        _userReadRepository = userReadRepository;
    }

    public async Task<UserProfileDto> Handle(
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var user = await _userReadRepository.
            GetProfileAsync(currentUserId, ct);

        if(user is null)
            throw new NotFoundException("User not found.");

        return user;
    }
}