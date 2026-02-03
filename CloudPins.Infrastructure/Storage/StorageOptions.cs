namespace CloudPins.Infrastructure.Storage;

public class StorageOptions
{
    public string BucketName { get; set; } = string.Empty;
    public string ServiceUrl { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
}