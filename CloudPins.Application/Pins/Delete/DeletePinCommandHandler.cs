using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.Delete;

public sealed class DeletePinCommandHandler
{
    private readonly IPinRepository _pinRepository;
    private readonly IPinSearchService _pinSearchService;
    private readonly IUnitOfWork _unitOfWork;

    public DeletePinCommandHandler(
        IPinRepository pinRepository,
        IPinSearchService pinSearchService,
        IUnitOfWork unitOfWork
    )
    {
        _pinRepository = pinRepository;
        _pinSearchService = pinSearchService;
        _unitOfWork = unitOfWork;
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
    }
}