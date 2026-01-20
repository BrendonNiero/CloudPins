namespace CloudPins.Application.Boards.Create;

public class CreateBoardCommand
{
    public string Name { get; init; } = string.Empty;
    public bool IsPublic { get; init; }
}