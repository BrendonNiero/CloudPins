using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class BoardRepository : IBoardRepository
{
    private readonly CloudPinsDbContext _context;

    public BoardRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task<Board?> GetBoardAsync(Guid id, CancellationToken ct)
    {
        return await _context.Boards
            .FirstAsync(b => b.Id == id && !b.IsDeleted, ct);
    }

    public async Task AddAsync(Board board, CancellationToken ct)
    {
        await _context.Boards.AddAsync(board, ct);
    }

    public async Task<bool> ExistsAsync(Guid boardId, CancellationToken ct)
    {
        return await _context.Boards
            .AnyAsync(b => b.Id == boardId && !b.IsDeleted, ct);
    }

}