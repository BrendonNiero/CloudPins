using CloudPins.Domain.Pins;

namespace CloudPins.Domain.Tests.Builders;

public class PinBuilder
{
    private Guid _ownerId = Guid.NewGuid();
    private Guid _boardId = Guid.NewGuid();
    private string _imageUrl = "storage/uploads/img.png";
    private string _thumbnailUrl = "storage/uploads/thumb.png";
    private string _title = "My Title";
    private string _description = "My description";
    private List<Guid> _tagIds = new () { Guid.NewGuid() };

    public PinBuilder WithTitle(string title)
    {
        _title = title;
        return this;
    }
    public PinBuilder WithImageUrl(string imageUrl)
    {
        _imageUrl = imageUrl;
        return this;
    }
    public PinBuilder WithThumbnailUrl(string thumbnailUrl)
    {
        _thumbnailUrl = thumbnailUrl;
        return this;
    }
    public PinBuilder WithOwner(Guid ownerId)
    {
        _ownerId = ownerId;
        return this;
    }

    public PinBuilder WithBoard(Guid boardId)
    {
        _boardId = boardId;
        return this;
    }

    public PinBuilder WithTags(params Guid[] tagIds)
    {
        _tagIds = tagIds.ToList();
        return this;
    }

    public PinBuilder WithoudTags()
    {
        _tagIds = new List<Guid>();
        return this;
    }

    public Pin Build()
    {
        return Pin.Create(
            _ownerId,
            _boardId,
            _imageUrl,
            _thumbnailUrl,
            _title,
            _description,
            _tagIds
        );
    }
}