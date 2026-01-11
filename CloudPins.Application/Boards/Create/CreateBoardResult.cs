namespace CloudPins.Application.Boards.Create;

public class CreateBoardResult
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool IsPublic { get; init; }
}