using CloudPins.Application.Common.Interfaces;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class PinReadRepository : IPinReadRepository
{
    private readonly CloudPinsDbContext _context;
}