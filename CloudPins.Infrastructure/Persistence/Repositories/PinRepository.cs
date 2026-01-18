using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Pins;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class PinRepository : IPinRepository
{
    private readonly CloudPinsDbContext _context;

    public PinRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Pin pin, CancellationToken ct)
    {
        await _context.Pins.AddAsync(pin, ct);
    }

    public async Task<Pin?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        return await _context.Pins
            .FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, ct);
    }
}