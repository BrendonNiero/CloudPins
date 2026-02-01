namespace CloudPins.Application.Pins.GetFeed;

public record FeedByPinQuery (Guid PinId, int Page = 1, int PageSize = 20);