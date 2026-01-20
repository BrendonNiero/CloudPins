using CloudPins.Domain.Common;

namespace CloudPins.Domain.Boards;

public class Board : BaseEntity
{
    public Guid OwnerId { get; private set; }
    public string Name { get; private set; } = string.Empty;
    public bool IsPublic { get; private set; }

    protected Board(){}

    public static Board Create (Guid ownerId, 
        string name, 
        bool isPublic)
    {
        var board = new Board
        {            
            Id = Guid.NewGuid(),
            OwnerId = ownerId,
            Name = name,
            IsPublic = isPublic,
            CreatedAt = DateTime.UtcNow
        };

        return board;
    }

    public void Rename(string name, string description)
    {
        Name = name;
    }

    public void ChangeVisibility(bool isPublic)
    {
        IsPublic = isPublic;
    }
}