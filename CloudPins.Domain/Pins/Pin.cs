using CloudPins.Domain.Common;
using CloudPins.Domain.Tags;

namespace CloudPins.Domain.Pins;

public class Pin : BaseEntity
{
    public Guid OwnerId { get; private set; }
    public Guid BoardId { get; private set; }
    public string ImageUrl { get; private set; } = string.Empty;
    public string ThumbnailUrl { get; private set; } = string.Empty;
    public string Title { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    private readonly List<Tag> _tags = new();
    public IReadOnlyCollection<Tag> Tags => _tags;
    public int LikesCount { get; private set; }
    protected Pin (){}

    public Pin(Guid ownerId, Guid boardId, string imageUrl, 
        string thumbnailUrl, string title, string description)
    {
        Id = Guid.NewGuid();
        OwnerId = ownerId;
        BoardId = boardId;
        ImageUrl = imageUrl;
        ThumbnailUrl = thumbnailUrl;
        Title = title;
        Description = description;
        LikesCount = 0;
        CreatedAt = DateTime.UtcNow;
    }

    public void AddLike() => LikesCount++;
    public void RemoveLike() => LikesCount--;

    public void AddTag(Tag tag)
    {
        if(_tags.Any(t => t.Id == tag.Id)) return;
        _tags.Add(tag);
    }
}