using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudPins.Application.Pins.Delete;

public sealed class DeletePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly IPinSearchService _pinSearchService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeletePinCommandHandler> _logger;

    public DeletePinCommandHandler(
        IPinRepository pinRepository,
        IPinSearchService pinSearchService,
        IUnitOfWork unitOfWork,
        ILogger<DeletePinCommandHandler> logger
    )
    {
        _pinRepository = pinRepository;
        _pinSearchService = pinSearchService;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task Handle(
        DeletePinCommand command,
        Guid currentUserId,
        CancellationToken ct
    )
    {
        var pin = await _pinRepository.GetByIdAsync(command.PinId, ct);

        if(pin is null)
            throw new NotFoundException("Pin not found.");

        if(pin.OwnerId != currentUserId)
            throw new ForbiddenException("You cannot delete this pin.");

        pin.SoftDelete();

        await _unitOfWork.SaveChangesAsync(ct);

        await _pinSearchService.DeleteAsync(pin.Id, ct);

        _logger.LogInformation(
            "Pin deleted and removed from search index. PinId {PinId}, OwnerId {OwnerId}",
            pin.Id,
            currentUserId);
    }
}
