namespace CloudPins.Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(
        Stream fileStream,
        string fileName,
        string contentType,
        CancellationToken ct
    );

    Task DeleteAsync(string fileKey, CancellationToken ct);
}