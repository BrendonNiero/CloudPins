namespace CloudPins.Application.Common.Interfaces;

public interface IStorageService
{
    Task<string> UploadAsync(
        byte[] bytes,
        string contentType,
        CancellationToken ct
    );

    Task<string> UploadProfileAsync(
        Guid userId,
        byte[] bytes,
        string contentType,
        CancellationToken ct
    );

    Task DeleteAsync(string fileKey, CancellationToken ct);
}