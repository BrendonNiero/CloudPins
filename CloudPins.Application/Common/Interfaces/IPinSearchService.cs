using CloudPins.Application.Pins.Search;
using CloudPins.Domain.Pins;

namespace CloudPins.Application.Common.Interfaces;

public interface IPinSearchService
{
    Task<IReadOnlyCollection<string>> AutocompleteAsync(
        string searchTerm,
        int size = 8,
        CancellationToken ct = default
    );
    Task<PinSearchResult> SearchAsync(
        string searchTerm,
        int page = 1,
        int pageSize = 20,
        CancellationToken ct = default
    );

    Task IndexAsync(
        Pin pin,
        IEnumerable<string> tagNames,
        CancellationToken ct = default
    );

    Task UpdateAsync(
        Pin pin,
        IEnumerable<string> tagNames,
        CancellationToken ct = default
    );

    Task DeleteAsync(
        Guid pinId,
        CancellationToken ct = default
    );
}
