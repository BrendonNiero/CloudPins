using CloudPins.Application.Common.Interfaces;
using Microsoft.Extensions.Configuration;

namespace CloudPins.Infrastructure.Storage;

public class FileSystemStorageService : IStorageService
{
    private readonly string _root;
    private readonly string _bucket;

    public FileSystemStorageService(IConfiguration config)
    {
        _root = config["Storage:LocalRoot"]!;
        _bucket = config["Storage:BucketName"]!;
    }

    public async Task<string> UploadAsync(
        byte[] bytes,
        string contentType,
        CancellationToken ct
    )
    {
        var extension = contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new InvalidOperationException("Tipo de imagem não suportada.")    
        };

        var key = $"pins/{Guid.NewGuid()}{extension}";
        var fullPath = Path.Combine(_root, _bucket, key);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await File.WriteAllBytesAsync(fullPath, bytes, ct);

        return $"/uploads/{key}";
    }

    public async Task<string> UploadProfileAsync(
        Guid userId,
        byte[] bytes,
        string contentType,
        CancellationToken ct
    )
    {
        var extension = contentType switch
        {
            "image/jpeg" => ".jpg",
            "image/jpg" => ".jpg",
            "image/png" => ".png",
            "image/webp" => ".webp",
            _ => throw new InvalidOperationException("Tipo de imagem não suportada.")    
        };

        var key = $"profiles/{userId}{extension}";
        var fullPath = Path.Combine(_root, _bucket, key);

        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

        await File.WriteAllBytesAsync(fullPath, bytes, ct);
        return $"/uploads/{key}";
    }

    public async Task DeleteAsync(string fileKey, CancellationToken ct)
    {
        var fullPath = Path.Combine(_root, _bucket, fileKey);

        if(File.Exists(fullPath))
            File.Delete(fullPath);

        await Task.CompletedTask;
    }
}