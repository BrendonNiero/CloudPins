using CloudPins.Domain.Likes;

namespace CloudPins.Application.Common.Interfaces;

public interface ILikeRepository
{
    Task<bool> ExistsAsync(Guid userId, Guid pinId);
    Task AddAsync(Like like);
    Task RemoveAsync(Guid userId, Guid pinId);
}