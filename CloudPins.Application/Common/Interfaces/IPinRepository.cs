using CloudPins.Domain.Pins;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinRepository
{
    Task<Pin?> GetByIdAsync(Guid id);
    Task AddAsync(Pin pin);
    Task UpdateAsync(Pin pin);
}