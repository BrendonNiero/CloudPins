namespace CloudPins.Domain.Likes;

public class Like
{
    public Guid UserId { get; private set; }
    public Guid PinId { get; private set; }
    public DateTime CreatedAt { get; private set; }

    protected Like() {}

    public Like(Guid userId, Guid pinId)
    {
        UserId = userId;
        PinId = pinId;
        CreatedAt = DateTime.UtcNow;
    }
}