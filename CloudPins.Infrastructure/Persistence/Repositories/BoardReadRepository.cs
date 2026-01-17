using CloudPins.Application.Boards.GetById;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class BoardReadRepository : IBoardReadRepository
{
    private readonly CloudPinsDbContext _context;

    public BoardReadRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task<BoardDetailsDto?> GetByIdAsync(Guid boardId, CancellationToken ct)
    {
        return await _context.Boards
        .AsNoTracking()
        .Where(b => b.Id == boardId && !b.IsDeleted)
        .Select(b => new BoardDetailsDto
        {
            Id = b.Id,
            OwnerId = b.OwnerId,
            Name = b.Name,
            Description = b.Description,
            IsPublic = b.IsPublic,
            CreatedAt = b.CreatedAt,
            PinIds = _context.Pins
            .Where(p => p.BoardId == boardId && !p.IsDeleted)
            .Select(p => p.Id)
            .ToList()
        })
        .FirstOrDefaultAsync(ct);
    }

    public async Task<List<BoardDetailsDto>> GetAllByUserAsync(
        Guid userId,
        CancellationToken ct
    )
    {
        return await _context.Boards
        .AsNoTracking()
        .Where(b => b.OwnerId == userId && !b.IsDeleted)
        .OrderByDescending(b => b.CreatedAt)
        .Select(b => new BoardDetailsDto
        {
            Id = b.Id,
            OwnerId = b.OwnerId,
            Name = b.Name,
            Description = b.Description,
            IsPublic = b.IsPublic,
            CreatedAt = b.CreatedAt,

            PinIds = _context.Pins
            .Where(p => p.BoardId == b.Id && !p.IsDeleted)
            .Select(p => p.Id)
            .ToList()
        })
        .ToListAsync(ct);
    }
}