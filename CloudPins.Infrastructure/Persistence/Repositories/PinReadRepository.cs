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
            Title = p.Title,
            BoardId = p.BoardId,
            OwnerId = p.OwnerId,
            ImageUrl = p.ImageUrl,
            ThumbnailUrl = p.ThumbnailUrl,
            Description = p.Description,
            LikesCount = p.LikesCount,
            TagIds = p.PinTags.Select(pt => pt.TagId).ToList(),
            CreatedAt = p.CreatedAt
        })
        .FirstOrDefaultAsync(ct);
    }

    public async Task<List<PinFeedItemDto>> GetFeedAsync(
        int page, 
        int pageSize, 
        CancellationToken ct)
    {
        var publicBoardIds = _context.Boards
            .AsNoTracking()
            .Where(b => b.IsPublic && !b.IsDeleted)
            .Select(b => b.Id);
        
        return await _context.Pins
            .AsNoTracking()
            .Where(p => 
                !p.IsDeleted &&
                publicBoardIds.Contains(p.BoardId))
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize * 3)
            .Take(pageSize * 3)
            .Select(p => new PinFeedItemDto
            {
                Id = p.Id,
                ThumbnailUrl = p.ThumbnailUrl
            })
            .ToListAsync(ct)
            .ContinueWith(t =>
                t.Result
                .OrderBy(_ => Random.Shared.Next())
                .Take(pageSize)
                .ToList()
            );
    }

    public async Task<List<PinFeedItemDto>> GetFeedByPinAsync(
        Guid pinId,
        int page,
        int pageSize,
        CancellationToken ct
    )
    {
        var tagIds = await _context.Pins
            .AsNoTracking()
            .Where(p => p.Id == pinId && !p.IsDeleted)
            .Select(p => p.PinTags.Select(pt => pt.TagId).ToList()).FirstOrDefaultAsync(ct);

        if(tagIds is null || tagIds.Count == 0)
        {
            return await GetFeedAsync(page, pageSize, ct);
        }

        var publicBoardIds = _context.Boards
            .AsNoTracking()
            .Where(b => b.IsPublic && !b.IsDeleted)
            .Select(b => b.Id);

            return await _context.Pins
                .AsNoTracking()
                .Where(p => !p.IsDeleted &&
                    p.Id != pinId &&
                    publicBoardIds.Contains(p.BoardId)
                )
                .Select(p => new
                {
                    Pin = p,
                    MatchCount = p.PinTags.Select(pt => pt.TagId).Count(t => tagIds.Contains(t))
                })
                .Where(x => x.MatchCount > 0)
                .OrderByDescending(x => x.MatchCount)
                .ThenByDescending(x => x.Pin.CreatedAt)
                .Skip((page - 1) * pageSize * 3)
                .Take(pageSize * 3)
                .Select(x => new PinFeedItemDto
                {
                    Id = x.Pin.Id,
                    ThumbnailUrl = x.Pin.ThumbnailUrl
                })
                .ToListAsync(ct)
                .ContinueWith(t =>
                    t.Result
                        .OrderBy(_ => Random.Shared.Next())
                        .Take(pageSize)
                        .ToList(), ct);

    }

    public async Task<List<PinBoardItemDto>> GetByBoardAsync(
        Guid boardId,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        return await _context.Pins
        .AsNoTracking()
        .Where(p => p.BoardId == boardId)
        .OrderByDescending(p => p.CreatedAt)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .Select(p => new PinBoardItemDto
        {
            Id = p.Id,
            ThumbnailUrl = p.ThumbnailUrl
        })
        .ToListAsync(ct);
    }
}