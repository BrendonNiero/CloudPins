using CloudPins.Application.Boards.GetById;

public interface IBoardReadRepository
{
    Task<BoardDetailsDto?> GetByIdAsync(Guid boardId, CancellationToken ct);
}