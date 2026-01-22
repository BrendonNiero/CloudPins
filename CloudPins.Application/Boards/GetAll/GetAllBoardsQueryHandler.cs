namespace CloudPins.Application.Boards.GetAll;

public class GetAllBoardsQueryHandler
{
    private readonly IBoardReadRepository _boardReadRepository;

    public GetAllBoardsQueryHandler(IBoardReadRepository boardReadRepository)
    {
        _boardReadRepository = boardReadRepository;
    }

    public async Task Handle(){}
}