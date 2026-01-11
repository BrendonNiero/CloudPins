namespace CloudPins.Application.Pins.Create;

public class CreatePinResult
{
    public Guid Id { get; init; }
    public Guid BoardId { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string ThumbNailUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
}