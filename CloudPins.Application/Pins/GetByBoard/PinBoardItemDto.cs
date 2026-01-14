namespace CloudPins.Application.Pins.GetByBoard;

public class PinBoardItemDto
{
    public Guid Id { get; init; }
    public string ThumbnailUrl { get; init; } = string.Empty;
}