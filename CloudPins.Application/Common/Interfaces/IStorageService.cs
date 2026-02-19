namespace CloudPins.Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(
        string key,
        byte[] bytes,
        string contentType,
        CancellationToken ct
    );

    Task DeleteAsync(string fileKey, CancellationToken ct);
}