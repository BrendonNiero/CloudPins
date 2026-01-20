namespace CloudPins.Application.Boards.GetAll;

public class BoardListItemDto
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsPublic { get; init; }
    public IReadOnlyCollection<BoardListDto> LasPins { get; init; } = [];
}