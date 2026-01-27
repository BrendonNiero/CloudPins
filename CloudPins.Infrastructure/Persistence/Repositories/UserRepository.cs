using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly CloudPinsDbContext _context;

    public UserRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(User user, CancellationToken ct)
    {
        await _context.Users.AddAsync(user, ct);
    }

    public async Task<User?> GetByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted, ct);
    }

    public async Task<bool> ExistsByEmailAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .AnyAsync(u => u.Email == email && !u.IsDeleted, ct);
    }
}