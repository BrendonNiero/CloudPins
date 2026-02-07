namespace CloudPins.Application.Pins.Create;

public class CreatePinCommand
{
    public Guid BoardId { get; init; }
    public byte[] ImageBytes { get; init; } = default!;
    public string ImageFileName { get; init; } = string.Empty;
    public string ImageContentType { get; init; } = string.Empty;

    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<Guid> TagIds { get; init; } = [];   
} 