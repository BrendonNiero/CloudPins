using CloudPins.Application.Tags.GetAll;

namespace CloudPins.Application.Common.Interfaces;

public interface ITagReadRepository
{
    Task<List<TagDto>> GetAllAsync(CancellationToken ct);
    Task<bool> ExistsAsync(string name, CancellationToken ct);
}