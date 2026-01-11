namespace CloudPins.Application.Pins.Create;

public class CreatePinCommand
{
    public Guid BoardId { get; init; }
    public string ImageUrl { get; init; } = string.Empty;
    public string ThumbNailUrl { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<Guid> TagIds { get; init; } = [];   
}