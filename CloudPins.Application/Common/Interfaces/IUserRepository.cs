using CloudPins.Domain.Users;

namespace CloudPins.Application.Common.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(Guid id, CancellationToken ct);
    Task UpdateAsync(User user, CancellationToken ct);
}