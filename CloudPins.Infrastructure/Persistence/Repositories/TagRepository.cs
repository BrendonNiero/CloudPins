using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Tags;

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

}