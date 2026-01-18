using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;

namespace CloudPins.Application.Boards.Create;

public class CreateBoardCommandHandler
{
    private IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateBoardCommandHandler(IBoardRepository boardRepository, IUnitOfWork unitOfWork)
    {
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
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

       await _unitOfWork.SaveChangesAsync(ct);
       return new CreateBoardResult
       {
         Id = board.Id,
         Name = board.Name,
         IsPublic = board.IsPublic  
       };
    }
}