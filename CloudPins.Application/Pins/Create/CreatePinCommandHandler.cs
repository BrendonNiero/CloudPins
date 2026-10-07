using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Pins;

namespace CloudPins.Application.Pins.Create;

public class CreatePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly IBoardRepository _boardRepository;
    private readonly IStorageService _storage;
    private readonly ITagRepository _tagRepository;
    private readonly IPinSearchService _pinSearchService;
    private readonly ISuggestionService _suggestionService;
    private readonly IUnitOfWork _unitOfWork;

    public CreatePinCommandHandler(
        IPinRepository pinRepository,
        IBoardRepository boardRepository,
        IStorageService storage,
        ITagRepository tagRepository,
        IPinSearchService pinSearchService,
        ISuggestionService suggestionService,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _boardRepository = boardRepository;
        _storage = storage;
        _tagRepository = tagRepository;
        _pinSearchService = pinSearchService;
        _suggestionService = suggestionService;
        _unitOfWork = unitOfWork;
    }

    public async Task<CreatePinResult> Handle(
        CreatePinCommand command,
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var tagIds = command.TagIds ?? new List<Guid>();

        var boardExists = await _boardRepository.ExistsAsync(command.BoardId, ct);
        if (!boardExists)
            throw new NotFoundException("Board not found.");
            

        var imageUrl = await _storage.UploadAsync(
            command.ImageBytes,
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
            tagIds: tagIds
        );

        await _pinRepository.AddAsync(pin, ct);
        await _unitOfWork.SaveChangesAsync(ct);


        var tagNames = await _tagRepository.GetNamesByIdsAsync(tagIds, ct);

        await _pinSearchService.IndexAsync(pin, tagNames, ct);
        try
        {
            await _suggestionService.ReindexAsync(ct);
        }
        catch (Exception)
        {
        }


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
