using CloudPins.Application.Boards.GetById;

public interface IBoardReadRepository
{
    Task<BoardDetailsDto?> GetByIdAsync(Guid boardId, CancellationToken ct);
    Task<List<BoardDetailsDto>> GetAllByUserAsync(
        Guid userId,
        CancellationToken ct
    );
}