using CloudPins.Application.Pins.GetById;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinReadRepository
{
    Task<PinDetailsDto?> GetByIdAsync(Guid pinId, CancellationToken ct);
}