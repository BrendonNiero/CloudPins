using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Likes;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class LikeRepository : ILikeRepository
{
    private readonly CloudPinsDbContext _context;

    public LikeRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Like like)
    {
        await _context.Likes.AddAsync(like);
    }

    public async Task RemoveAsync(Guid userId, Guid pinId)
    {
        var like = await _context.Likes.FirstOrDefaultAsync(l => l.UserId == userId && l.PinId == pinId);

        if(like is null) return;

        _context.Likes.Remove(like);
    }
    public async Task<bool> ExistsAsync(Guid userId, Guid pinId)
    {
        return await _context.Likes
            .AnyAsync(l => l.UserId == userId && l.PinId == pinId);
    }
}