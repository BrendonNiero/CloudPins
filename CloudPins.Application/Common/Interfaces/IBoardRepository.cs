using CloudPins.Domain.Boards;

namespace CloudPins.Application.Common.Interfaces;

public interface IBoardRepository
{
    Task<Board?> GetBoardAsync(Guid id);
    Task AddAsync(Board board);
}