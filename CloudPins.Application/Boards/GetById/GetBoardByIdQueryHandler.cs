using CloudPins.Application.Common.Exceptions;

namespace CloudPins.Application.Boards.GetById;

public class GetBoardByIdQueryHandler
{
    private readonly IBoardReadRepository _boardReadRepository;

    public GetBoardByIdQueryHandler(IBoardReadRepository boardReadRepository)
    {
        _boardReadRepository = boardReadRepository;
    }

    public async Task<BoardDetailsDto> Handle(
        GetBoardByIdQuery query,
        CancellationToken ct
    )
    {
        var board = await _boardReadRepository
            .GetByIdAsync(query.BoardId, ct);

        if(board is null)
            throw new NotFoundException("Board not found.");

        return board;
    }
}