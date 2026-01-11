using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;

namespace CloudPins.Application.Boards.Create;

public class CreateBoardCommandHandler
{
    private IBoardRepository _boardRepository;

    public CreateBoardCommandHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<CreateBoardResult> Handle(
        CreateBoardCommand command,
        Guid currentUserId,
        CancellationToken ct)
    {
       var board = Board.Create(
        ownerId: currentUserId,
        name: command.Name,
        description: command.Description ?? "",
        isPublic: command.IsPublic
       );

       await _boardRepository.AddAsync(board, ct);

       return new CreateBoardResult
       {
         Id = board.Id,
         Name = board.Name,
         IsPublic = board.IsPublic  
       };
    }
}