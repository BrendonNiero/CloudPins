namespace CloudPins.Application.Pins.GetAll;

public class GetPinsFeedQuery
{
    public int Page { get; init; } = 1;
    public int PageSize { get; init; } = 20;
};