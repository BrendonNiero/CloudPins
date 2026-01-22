using CloudPins.Application.Boards.GetAll;
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
        (
            b.Id,
            b.IsPublic,
            _context.Pins
            .Where(p => p.BoardId == boardId && !p.IsDeleted)
            .Select(p => new BoardPinItemDto
            (
                p.Id,
                p.ThumbnailUrl
            ))
            .ToList()
        ))
        .FirstOrDefaultAsync(ct);
    }

    public async Task<IReadOnlyCollection<BoardListItemDto>> GetAllAsync(
        Guid userId,
        CancellationToken ct
    )
    {
        var boards = await _context.Boards
            .AsNoTracking()
            .Where(b => !b.IsDeleted)
            .OrderByDescending(b => b.CreatedAt)
            .Select(b => new
            {
                b.Id,
                b.Name,
                b.IsPublic,
                b.CreatedAt
            })
            .ToListAsync(ct);

        var boardIds = boards.Select(b => b.Id).ToList();

        var pins = await _context.Pins
                    .AsNoTracking()
                    .Where(p => boardIds.Contains(p.BoardId) && !p.IsDeleted)
                    .OrderByDescending(p => p.CreatedAt)
                    .ToListAsync(ct);

        return boards.Select(b =>
        {
            var boardPins = pins.Where(p => p.BoardId == b.Id)
                            .Take(3).ToList();

            return new BoardListItemDto(
                b.Id,
                b.Name,
                b.IsPublic,
                boardPins.Select(p => new BoardLastPinDto(p.ThumbnailUrl))
                        .ToList(),
                pins.Count,
                b.CreatedAt
            );
        }).ToList();
    }
}