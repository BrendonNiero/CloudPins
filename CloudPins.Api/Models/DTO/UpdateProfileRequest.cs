namespace CloudPins.Api.Models.DTO;

public class UpdateProfileRequest
{
    public IFormFile Image { get; set; } = default!;
    public string Name { get; set; } = string.Empty;
}