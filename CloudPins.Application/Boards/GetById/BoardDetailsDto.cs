namespace CloudPins.Application.Boards.GetById;

public class BoardDetailsDto
{
    public Guid Id { get; init; }
    public Guid OwnerId { get; init; }
    public string Name { get; init; } = string.Empty;
    public string? Description { get; init; }
    public bool IsPublic { get; init; }

    public IReadOnlyCollection<Guid> PinIds { get; init; } = [];
}