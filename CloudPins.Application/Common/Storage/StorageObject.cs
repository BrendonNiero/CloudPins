namespace CloudPins.Application.Common.Storage;

public record StorageObject(
    byte[] Bytes,
    string ContentType
);