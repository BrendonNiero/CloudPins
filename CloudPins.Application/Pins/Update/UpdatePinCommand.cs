namespace CloudPins.Application.Pins.Update;

public sealed class UpdatePinCommand
{
    public Guid PinId { get; init; }
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<Guid> TagIds { get; init; } = [];
}