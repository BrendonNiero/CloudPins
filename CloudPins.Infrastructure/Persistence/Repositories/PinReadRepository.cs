using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Pins.GetById;
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
}