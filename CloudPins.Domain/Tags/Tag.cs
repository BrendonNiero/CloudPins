using CloudPins.Domain.Common;
namespace CloudPins.Domain.Tags;
public class Tag : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    protected Tag() {}

    public Tag(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }
}