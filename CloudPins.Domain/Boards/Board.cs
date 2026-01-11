using CloudPins.Domain.Common;

namespace CloudPins.Domain.Boards;

public class Board : BaseEntity
{
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public string Description { get; private set; } = string.Empty;
    public bool IsPublic { get; private set; }

    protected Board(){}

    public static Board Create (Guid ownerId, 
        string name, 
        string description, 
        bool isPublic)
    {
        var board = new Board
        {            
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Name = name,
            Description = description,
            IsPublic = isPublic,
            CreatedAt = DateTime.UtcNow
        };

        return board;
    }

    public void Rename(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void ChangeVisibility(bool isPublic)
    {
        IsPublic = isPublic;
    }
}