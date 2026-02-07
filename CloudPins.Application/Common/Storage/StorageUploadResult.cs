namespace CloudPins.Application.Common.Storage;

public record StorageUploadResult(
    string Bucket,
    string Key,
    string PublicUrl
);