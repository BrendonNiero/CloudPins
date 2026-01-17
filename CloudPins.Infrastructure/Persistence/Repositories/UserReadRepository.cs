using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Users.Get;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class UserReadRepository : IUserReadRepository
{
    private readonly CloudPinsDbContext _context;

    public UserReadRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task<UserProfileDto?> GetProfileAsync(Guid id, CancellationToken ct)
    {
        return await _context.Users
        .AsNoTracking()
        .Where(u => u.Id == id && !u.IsDeleted)
        .Select(u => new UserProfileDto(
            u.Id,
            u.Name,
            u.Email,
            u.ProfileUrl
        ))
        .FirstOrDefaultAsync(ct);
    }
}