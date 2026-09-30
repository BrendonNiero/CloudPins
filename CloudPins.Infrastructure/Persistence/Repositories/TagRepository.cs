using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Tags;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence.Repositories;

public class TagRepository : ITagRepository
{
    private readonly CloudPinsDbContext _context;
    public TagRepository(CloudPinsDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Tag tag, CancellationToken ct)
    {
        await _context.Tags.AddAsync(tag, ct);
    }

    public async Task<IReadOnlyCollection<string>> GetNamesByIdsAsync(
        IEnumerable<Guid> tagIds,
        CancellationToken ct
    )
    {
        var ids = tagIds.Distinct().ToArray();

        if(ids.Length == 0)
            return Array.Empty<string>();

        return await _context.Tags
            .Where(tag => ids.Contains(tag.Id))
            .Select(tag => tag.Name)
            .ToArrayAsync(ct);
    }
}