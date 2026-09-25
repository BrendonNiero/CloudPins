using CloudPins.Domain.Pins;

namespace CloudPins.Infrastructure.Search;

public static class PinDocumentMapper
{
    public static PinDocument ToDocument(
        Pin pin,
        IEnumerable<string> tagNames
    )
    {
        ArgumentNullException.ThrowIfNull(pin);
        ArgumentNullException.ThrowIfNull(tagNames);

        return new PinDocument
        {
            Id = pin.Id,
            OwnerId = pin.OwnerId,
            BoardId = pin.BoardId,
            ImageUrl = pin.ImageUrl,
            ThumbnailUrl = pin.ThumbnailUrl,
            Title = pin.Title,
            Description = pin.Description,
            Tags = tagNames
                .Where(tag => !string.IsNullOrWhiteSpace(tag))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToArray(),
            LikesCount = pin.LikesCount,
            CreatedAt = pin.CreatedAt
        };
    }
}