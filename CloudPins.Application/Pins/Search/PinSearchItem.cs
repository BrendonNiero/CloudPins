namespace CloudPins.Application.Pins.Search;

public sealed class PinSearchItem
{
    public Guid Id { get; init; }
    public string ThumbnailUrl { get; init; } = string.Empty;
    public string ImageUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Tags { get; init; } = Array.Empty<string>();
}