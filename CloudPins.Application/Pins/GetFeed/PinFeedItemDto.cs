namespace CloudPins.Application.Pins.GetFeed;

public class PinFeedItemDto
{
    public Guid Id { get; init; }
    public string ThumbnailUrl { get; init; } = string.Empty;
}