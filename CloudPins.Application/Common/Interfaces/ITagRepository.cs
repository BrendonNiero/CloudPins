using CloudPins.Domain.Tags;

namespace CloudPins.Application.Common.Interfaces;

public interface ITagRepository
{
    Task AddAsync(Tag tag, CancellationToken ct);
}