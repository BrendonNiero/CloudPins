using CloudPins.Domain.Tags;

namespace CloudPins.Application.Common.Interfaces;

public interface ITagRepository
{
    Task<List<Tag>> GetByIdsAsync(IEnumerable<Guid> ids);
    Task AddAsync(Tag tag, CancellationToken ct);
}