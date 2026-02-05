using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Pins;

namespace CloudPins.Application.Pins.Create;

public class CreatePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IStorageService _storage;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePinCommandHandler(
        IPinRepository pinRepository,
        IBoardRepository boardRepository,
        IStorageService storage,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _boardRepository = boardRepository;
        _storage = storage;
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

        var imageUrl = await _storage.UploadAsync(
            command.ImageStream,
            command.ImageFileName,
            command.ImageContentType,
            ct
        );

        var pin = Pin.Create(
            ownerId: currentUserId,
            boardId: command.BoardId,
            imageUrl: imageUrl,
            thumbNailUrl: imageUrl,
            title: command.Title,
            description: command.Description,
            tagIds: command.TagIds ?? Enumerable.Empty<Guid>()
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