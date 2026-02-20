namespace CloudPins.Application.Pins.GetById;

public class PinDetailsDto
{
    public Guid Id { get; init; }
    public Guid BoardId { get; init; }
    public Guid OwnerId { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string ThumbnailUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public bool IsLiked { get; init; }

    public IReadOnlyCollection<Guid> TagIds { get; init; } = [];
    public int LikesCount { get; init; }
    public DateTime CreatedAt { get; init; }
}