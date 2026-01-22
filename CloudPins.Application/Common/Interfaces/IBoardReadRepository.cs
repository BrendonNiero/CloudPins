using CloudPins.Application.Boards.GetAll;
using CloudPins.Application.Boards.GetById;

public interface IBoardReadRepository
{
    Task<BoardDetailsDto?> GetByIdAsync(Guid boardId, CancellationToken ct);
    Task<IReadOnlyCollection<BoardListItemDto>> GetAllAsync(
        Guid userId,
        CancellationToken ct
    );
}