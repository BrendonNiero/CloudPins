namespace CloudPins.Api.Models.DTO;

public class CreatePinRequest
{
    public Guid BoardId { get; set; }
    public IFormFile Image { get; set; } = default!;
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public List<Guid> TagIds { get; set; } = [];
}