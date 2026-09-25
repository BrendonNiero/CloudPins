namespace CloudPins.Infrastructure.Search;

public sealed class PinDocument
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public Guid BoardId { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string ThumbnailUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public IReadOnlyCollection<string> Tags { get; init; } = [];
    public int LikesCount { get; init; }
    public DateTime CreatedAt { get; init; }
}