namespace CloudPins.Domain.Pins;

public class PinTag
{
    public Guid PinId { get; init; }
    public Guid TagId { get; init; }

    protected PinTag() {}

    public PinTag(Guid pinId, Guid tagId)
    {
        PinId = pinId;
        TagId = tagId;
    }
}