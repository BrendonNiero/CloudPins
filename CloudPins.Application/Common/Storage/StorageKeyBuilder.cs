namespace CloudPins.Application.Common.Storage;

public static class StorageKeyBuilder
{
    public static string BuilderPinImageKey(Guid pinId, string fileName)
    {
        var ext = Path.GetExtension(fileName);
        return $"cloudpins/pins/{pinId:N}{ext}";
    }
}