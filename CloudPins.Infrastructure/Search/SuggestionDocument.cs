namespace CloudPins.Infrastructure.Search;

public sealed class SuggestionDocument
{
    public string Id { get; init; } = string.Empty;
    public string Text { get; init; } = string.Empty;
    public string NormalizedText { get; init; } = string.Empty;
    public int Frequency { get; init; }
    public string Source { get; init; } = string.Empty;
    public DateTime LastSeenAt { get; init; } 
}