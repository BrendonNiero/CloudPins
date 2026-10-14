using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Logging;

namespace CloudPins.Application.Pins.GetById;

public class GetPinByIdQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;
    private readonly ILogger<GetPinByIdQueryHandler> _logger;

    public GetPinByIdQueryHandler(
        IPinReadRepository pinReadRepository,
        ILogger<GetPinByIdQueryHandler> logger)
    {
        _pinReadRepository = pinReadRepository;
        _logger = logger;
    }

    public async Task<PinDetailsDto> Handle(
        GetPinByIdQuery query, 
        CancellationToken ct)
    {
        var pin = await _pinReadRepository
            .GetByIdAsync(query.PinId, query.CurrentUserId, ct);

        if(pin is null)
            throw new NotFoundException("Pin not found.");

        _logger.LogInformation(
            "Pin details loaded. PinId {PinId}, UserId {UserId}",
            query.PinId,
            query.CurrentUserId);

        return pin;
    }
}
