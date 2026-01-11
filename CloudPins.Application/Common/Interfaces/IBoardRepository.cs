using CloudPins.Domain.Boards;

namespace CloudPins.Application.Common.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetBoardAsync(Guid id, CancellationToken ct);
    Task AddAsync(Board board, CancellationToken ct);
    Task<bool> ExistsAsync(Guid boardId, CancellationToken ct);
}