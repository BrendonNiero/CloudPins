namespace CloudPins.Application.Pins.GetFeed;

public record SearchFeedQuery(string Search, int Page = 1, int PageSize = 20);