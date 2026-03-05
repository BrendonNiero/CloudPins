using CloudPins.Domain.Common;
namespace CloudPins.Domain.Tags;
public class Tag : BaseEntity
{
    public string Name { get; private set; } = string.Empty;

    protected Tag() {}

    public Tag(string name)
    {
        if(string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required");
        Id = Guid.NewGuid();
        Name = name;
        CreatedAt = DateTime.UtcNow;
    }
}