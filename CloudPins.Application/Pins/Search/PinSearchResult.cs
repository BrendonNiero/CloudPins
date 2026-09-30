namespace CloudPins.Application.Pins.Search;

public sealed class PinSearchResult
{
    public IReadOnlyCollection<PinSearchItem> Items { get; init; } =
        Array.Empty<PinSearchItem>();

    public int Page { get; init; }

    public int PageSize { get; init; }

    public long Total { get; init; }

    public int TotalPages =>
        PageSize == 0
            ? 0
            : (int)Math.Ceiling((double)Total / PageSize);
}
