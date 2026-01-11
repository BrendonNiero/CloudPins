using CloudPins.Application.Common.Exceptions;
using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Application.Pins.GetById;

public class GetPinByIdQueryHandler
{
    private readonly IPinReadRepository _pinReadRepository;

    public GetPinByIdQueryHandler(IPinReadRepository pinReadRepository)
    {
        _pinReadRepository = pinReadRepository;
    }

    public async Task<PinDetailsDto> Handle(GetPinByIdQuery query, CancellationToken ct)
    {
        var pin = await _pinReadRepository
            .GetByIdAsync(query.PinId, ct);

        if(pin is null)
            throw new NotFoundException("Pin not found.");

        return pin;
    }
}