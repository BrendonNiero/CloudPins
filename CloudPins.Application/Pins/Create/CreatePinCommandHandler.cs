using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Pins;

namespace CloudPins.Application.Pins.Create;

public class CreatePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly IBoardRepository _boardRepository;

    private readonly IUnitOfWork _unitOfWork;

    public CreatePinCommandHandler(
        IPinRepository pinRepository,
        IBoardRepository boardRepository,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _boardRepository = boardRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePinResult> Handle(
        CreatePinCommand command,
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var boardExists = await _boardRepository
        .ExistsAsync(command.BoardId, ct);

        if(!boardExists)
            throw new NotFoundException("Board not found.");

        var pin = Pin.Create(
            ownerId: currentUserId,
            boardId: command.BoardId,
            imageUrl: command.ImageUrl,
            thumbNailUrl: command.ThumbNailUrl,
            title: command.Title,
            description: command.Description,
            tagIds: command.TagIds
        );

        await _pinRepository.AddAsync(pin, ct);
        await _unitOfWork.SaveChangesAsync(ct);

        return new CreatePinResult
        {
            Id = pin.Id,
            BoardId = pin.BoardId,
            ImageUrl = pin.ImageUrl,
            ThumbNailUrl = pin.ThumbnailUrl,
            Title = pin.Title
        };
    }
}