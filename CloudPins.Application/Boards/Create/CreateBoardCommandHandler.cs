using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;
using Microsoft.Extensions.Logging;

namespace CloudPins.Application.Boards.Create;

public class CreateBoardCommandHandler
{
    private IBoardRepository _boardRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<CreateBoardCommandHandler> _logger;

    public CreateBoardCommandHandler(
        IBoardRepository boardRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateBoardCommandHandler> logger)
    {
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<CreateBoardResult> Handle(
        CreateBoardCommand command,
        Guid currentUserId,
        CancellationToken ct)
    {
       var board = Board.Create(
        ownerId: currentUserId,
        name: command.Name,
        isPublic: command.IsPublic
       );

       await _boardRepository.AddAsync(board, ct);

       await _unitOfWork.SaveChangesAsync(ct);

       _logger.LogInformation(
         "Board created. BoardId {BoardId}, OwnerId {OwnerId}, IsPublic {IsPublic}",
         board.Id,
         currentUserId,
         board.IsPublic);

       return new CreateBoardResult
       {
         Id = board.Id,
         Name = board.Name,
         IsPublic = board.IsPublic  
       };
    }
}
