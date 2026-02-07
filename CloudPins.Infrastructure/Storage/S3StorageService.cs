using Amazon.S3;
using Amazon.S3.Model;
using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Options;

namespace CloudPins.Infrastructure.Storage;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3;
    private readonly StorageOptions _options;

    public S3StorageService(
        IAmazonS3 s3,
        IOptions<StorageOptions> options
    )
    {
        _s3 = s3;
        _options = options.Value;
    }

    public async Task<string> UploadAsync(
        byte[] bytes,
        string contentType,
        CancellationToken ct
    )
    {
        var key = $"pins/{Guid.NewGuid()}";

        using var stream = new MemoryStream(bytes);

        var request = new PutObjectRequest
        {
            BucketName = _options.BucketName,
            Key = key,
            InputStream = stream,
            ContentType = contentType,
            CannedACL = S3CannedACL.PublicRead
        };

        await _s3.PutObjectAsync(request, ct);

        return $"{_options.ServiceUrl}/{_options.BucketName}/{key}";
    }

    public async Task DeleteAsync(string fileKey, CancellationToken ct)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _options.BucketName,
            Key = fileKey
        };

        await _s3.DeleteObjectAsync(request, ct);
    }
}