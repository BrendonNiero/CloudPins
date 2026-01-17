using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Tags.GetAll;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class TagReadRepository : ITagReadRepository
{
    private readonly CloudPinsDbContext _context;

    public TagReadRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task<List<TagDto>> GetAllAsync(CancellationToken ct)
    {
        return await _context.Tags
        .AsNoTracking()
        .Where(t => !t.IsDeleted)
        .OrderBy(t => t.Name)
        .Select(t => new TagDto(
            t.Id,
            t.Name
        ))
        .ToListAsync();
    }
}