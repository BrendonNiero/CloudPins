namespace CloudPins.Application.Boards.GetById;

public record BoardPinItemDto(
    Guid Id,
    string ThumbnailUrl
);