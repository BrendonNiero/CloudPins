namespace CloudPins.Application.Boards.GetAll;

public record BoardListItemDto(
    Guid Id, 
    string Name, 
    bool IsPublic, 
    IReadOnlyCollection<BoardLastPinDto> LastPins,
    int PinsCount,
    DateTime CreatedAt
);