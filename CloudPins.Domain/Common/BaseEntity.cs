namespace CloudPins.Domain.Common;

public abstract class BaseEntity
{
    public Guid Id { get; protected set; }
    public DateTime CreatedAt { get; protected set; }
    public bool IsDeleted { get; protected set; }
    public DateTime? DeletedAt { get; protected set; }

    public void SoftDelete()
    {
        if(IsDeleted) return;
        
        IsDeleted = true;
        DeletedAt = DateTime.UtcNow;
    }
}