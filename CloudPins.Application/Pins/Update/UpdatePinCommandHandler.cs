using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.Update;

public sealed class UpdatePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly ITagRepository _tagRepository;
    private readonly IPinSearchService _pinSearchService;
    private readonly ISuggestionService _suggestionService;
    private readonly IUnitOfWork _unitOfWork;

    public UpdatePinCommandHandler(
        IPinRepository pinRepository,
        ITagRepository tagRepository,
        IPinSearchService pinSearchService,
        ISuggestionService suggestionService,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _tagRepository = tagRepository;
        _pinSearchService = pinSearchService;
        _suggestionService = suggestionService;
        _unitOfWork = unitOfWork;
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
        try
        {
            await _suggestionService.ReindexAsync(ct);
        }
        catch (Exception)
        {
        }
    }
}
