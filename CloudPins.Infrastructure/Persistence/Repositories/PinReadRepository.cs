using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Pins.GetByBoard;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class PinReadRepository : IPinReadRepository
{
    private readonly CloudPinsDbContext _context;

    public PinReadRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task<PinDetailsDto?> GetByIdAsync(Guid pinId, CancellationToken ct)
    {
        return await _context.Pins
        .AsNoTracking()
        .Where(p => p.Id == pinId)
        .Select(p => new PinDetailsDto
        {
            Id = p.Id,
            BoardId = p.BoardId,
            OwnerId = p.OwnerId,
            ImageUrl = p.ImageUrl,
            ThumbnailUrl = p.ThumbnailUrl,
            Description = p.Description,
            LikesCount = p.LikesCount,
            TagIds = p.TagIds.ToList(),
            CreatedAt = p.CreatedAt
        })
        .FirstOrDefaultAsync(ct);
    }

    public async Task<List<PinFeedItemDto>> GetFeedAsync(Guid id, int page, int pageSize, CancellationToken ct)
    {
        return new List<PinFeedItemDto>();
    }

    public async Task<List<PinBoardItemDto>> GetByBoardAsync(Guid boardId, CancellationToken ct)
    {
        return new List<PinBoardItemDto>();
    }
}