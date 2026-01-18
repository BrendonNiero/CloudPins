using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Infrastructure.Persistence;

public sealed class UnitOfWork : IUnitOfWork
{
    private readonly CloudPinsDbContext _context;

    public UnitOfWork(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task SaveChangesAsync(CancellationToken ct)
    {
        await _context.SaveChangesAsync(ct);
    }
}