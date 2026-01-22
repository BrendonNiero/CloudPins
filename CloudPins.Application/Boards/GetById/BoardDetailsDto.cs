namespace CloudPins.Application.Boards.GetById;

public record BoardDetailsDto(
    Guid Id,
    bool IsPublic,
    IReadOnlyCollection<BoardPinItemDto> Pins
);