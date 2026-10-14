using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudPins.Application.Pins.Update;

public sealed class UpdatePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IPinSearchService _pinSearchService;
    private readonly ISuggestionReindexQueue _suggestionReindexQueue;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<UpdatePinCommandHandler> _logger;

    public UpdatePinCommandHandler(
        IPinRepository pinRepository,
        ITagRepository tagRepository,
        IPinSearchService pinSearchService,
        ISuggestionReindexQueue suggestionReindexQueue,
        IUnitOfWork unitOfWork,
        ILogger<UpdatePinCommandHandler> logger
    )
    {
        _pinRepository = pinRepository;
        _tagRepository = tagRepository;
        _pinSearchService = pinSearchService;
        _suggestionReindexQueue = suggestionReindexQueue;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        UpdatePinCommand command,
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var pin = await _pinRepository.GetByIdAsync(command.PinId, ct);

        if(pin is null)
            throw new NotFoundException("Pin not found.");

        if(pin.OwnerId != currentUserId)
            throw new ForbiddenException("You cannot update this pin.");

        var tagIds = command.TagIds ??[];

        pin.UpdateDetails(
            command.Title,
            command.Description,
            tagIds
        );

        await _unitOfWork.SaveChangesAsync(ct);

        var tagNames = await _tagRepository.GetNamesByIdsAsync(tagIds, ct);

        await _pinSearchService.UpdateAsync(pin, tagNames, ct);
        _suggestionReindexQueue.Enqueue();

        _logger.LogInformation(
            "Pin updated and reindexed. PinId {PinId}, OwnerId {OwnerId}, TagsCount {TagsCount}",
            pin.Id,
            currentUserId,
            tagIds.Count);
    }
}
