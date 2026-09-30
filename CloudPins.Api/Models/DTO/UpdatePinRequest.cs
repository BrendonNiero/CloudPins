namespace CloudPins.Api.Models.DTO;

public sealed class UpdatePinRequest
{
    public string Title { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public List<Guid> TagIds { get; init; } = [];
}