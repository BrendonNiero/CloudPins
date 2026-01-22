using CloudPins.Domain.Boards;

namespace CloudPins.Application.Common.Interfaces;

public interface IBoardRepository
{
    Task AddAsync(Board board, CancellationToken ct);
    Task<bool> ExistsAsync(Guid boardId, CancellationToken ct);
}