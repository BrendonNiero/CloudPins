using CloudPins.Domain.Common;
namespace CloudPins.Domain.Pins;

public class Pin : BaseEntity
{
    public Guid OwnerId { get; private set; }
    public Guid BoardId { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public string ThumbnailUrl { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    private readonly List<Guid> _tagIds = new();
    public IReadOnlyCollection<Guid> TagIds => _tagIds.AsReadOnly();
    public int LikesCount { get; private set; }
    protected Pin (){}

    public static Pin Create(
        Guid ownerId,
        Guid boardId,
        string imageUrl,
        string thumbNailUrl,
        string title,
        string description,
        IEnumerable<Guid> tagIds
    )
    {
        var pin = new Pin
        {
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            BoardId = boardId,
            ImageUrl = imageUrl,
            ThumbnailUrl = thumbNailUrl,
            Title = title,
            Description = description,
            LikesCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        foreach(var tagId in tagIds.Distinct())
        {
            pin.AddTag(tagId);
        }
        return pin;
    }

    public void AddLike() => LikesCount++;
    public void RemoveLike()
    {
        if(LikesCount > 0)
            LikesCount--;
    }

    public void AddTag(Guid tagId)
    {
        if(_tagIds.Contains(tagId)) return;
        _tagIds.Add(tagId);
    }
}