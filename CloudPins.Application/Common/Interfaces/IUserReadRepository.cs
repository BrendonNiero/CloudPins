using CloudPins.Application.Users.Get;

namespace CloudPins.Application.Common.Interfaces;

public interface IUserReadRepository
{
    Task<UserProfileDto?> GetProfileAsync(Guid id, CancellationToken ct);
}