using CloudPins.Domain.Pins;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinRepository
{
    Task<Pin?> GetByIdAsync(Guid id, CancellationToken ct);
    Task AddAsync(Pin pin, CancellationToken ct);
}