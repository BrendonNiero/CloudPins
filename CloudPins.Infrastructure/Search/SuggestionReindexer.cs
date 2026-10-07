using CloudPins.Domain.Pins;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Infrastructure.Persistence;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Text.RegularExpressions;

namespace CloudPins.Infrastructure.Search;

public sealed class SuggestionReindexer : ISuggestionService
{
    private const int MaxWordsPerSuggestion = 4;
    private static readonly SemaphoreSlim ReindexLock = new(1, 1);

    private readonly CloudPinsDbContext _context;
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchOptions _options;

    public SuggestionReindexer(
        CloudPinsDbContext context,
        ElasticsearchClient client,
        IOptions<ElasticsearchOptions> options)
    {
        _context = context;
        _client = client;
        _options = options.Value;
    }

    public async Task<int> ReindexAsync(
        CancellationToken cancellationToken = default)
    {
        await ReindexLock.WaitAsync(cancellationToken);

        try
        {
            return await ReindexCoreAsync(cancellationToken);
        }
        finally
        {
            ReindexLock.Release();
        }
    }

    private async Task<int> ReindexCoreAsync(
        CancellationToken cancellationToken)
    {
        var clearResponse = await _client.DeleteByQueryAsync<SuggestionDocument>(
            request => request
                .Indices("cloudpins_suggestions")
                .Query(query => query.MatchAll(new MatchAllQuery())),
            cancellationToken);

        if (!clearResponse.IsValidResponse)
        {
            throw new InvalidOperationException(
                "Could not clear the suggestions index.");
        }

        var pins = await _context.Pins
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var pinTags = await _context.Set<PinTag>()
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        var tags = await _context.Tags
            .AsNoTracking()
            .ToDictionaryAsync(
                tag => tag.Id,
                tag => tag.Name,
                cancellationToken);

        var tagNamesByPin = pinTags
            .GroupBy(pinTag => pinTag.PinId)
            .ToDictionary(
                group => group.Key,
                group => group
                    .Where(pinTag => tags.ContainsKey(pinTag.TagId))
                    .Select(pinTag => tags[pinTag.TagId])
                    .ToArray());

        var suggestions = new Dictionary<string, SuggestionData>(
            StringComparer.OrdinalIgnoreCase);

        foreach (var pin in pins)
        {
            var tagNames = tagNamesByPin.TryGetValue(
                pin.Id,
                out var names)
                ? names
                : Array.Empty<string>();

            AddSuggestions(
                suggestions,
                pin.Title,
                "pin-title");

            AddSuggestions(
                suggestions,
                pin.Description,
                "pin-description");

            foreach (var tagName in tagNames)
            {
                AddSuggestions(
                    suggestions,
                    tagName,
                    "pin-tag");
            }
        }

        foreach (var suggestion in suggestions.Values)
        {
            var document = new SuggestionDocument
            {
                Id = CreateId(suggestion.Text),
                Text = suggestion.Text,
                NormalizedText = Normalize(suggestion.Text),
                Frequency = suggestion.Frequency,
                Source = suggestion.Source,
                LastSeenAt = DateTime.UtcNow
            };

            var response = await _client.IndexAsync(
                document,
                request => request
                    .Index("cloudpins_suggestions")
                    .Id(document.Id),
                cancellationToken);

            if (!response.IsValidResponse)
            {
                throw new InvalidOperationException(
                    $"Could not index suggestion '{document.Text}'.");
            }
        }

        var refreshResponse = await _client.Indices.RefreshAsync(
            "cloudpins_suggestions",
            cancellationToken);

        if (!refreshResponse.IsValidResponse)
        {
            throw new InvalidOperationException(
                "Could not refresh the suggestions index.");
        }

        return suggestions.Count;
    }

    private static void AddSuggestions(
        Dictionary<string, SuggestionData> suggestions,
        string? text,
        string source)
    {
        if (string.IsNullOrWhiteSpace(text))
            return;

        var words = Tokenize(text);

        for (var start = 0; start < words.Length; start++)
        {
            for (
                var length = 1;
                length <= MaxWordsPerSuggestion &&
                start + length <= words.Length;
                length++)
            {
                var phrase = string.Join(
                    ' ',
                    words.Skip(start).Take(length));

                if (phrase.Length < 2)
                    continue;

                var normalizedPhrase = Normalize(phrase);

                if (suggestions.TryGetValue(
                    normalizedPhrase,
                    out var existing))
                {
                    existing.Frequency++;
                    continue;
                }

                suggestions[normalizedPhrase] = new SuggestionData
                {
                    Text = phrase,
                    Source = source,
                    Frequency = 1
                };
            }
        }
    }

    private static string[] Tokenize(string text)
    {
        return Regex
            .Replace(text.ToLowerInvariant(), @"[^\p{L}\p{N}\s]", " ")
            .Split(
                ' ',
                StringSplitOptions.RemoveEmptyEntries |
                StringSplitOptions.TrimEntries);
    }

    private static string Normalize(string value)
    {
        return Regex
            .Replace(value.ToLowerInvariant(), @"\s+", " ")
            .Trim();
    }

    private static string CreateId(string value)
    {
        return Normalize(value)
            .Replace(' ', '-');
    }

    private sealed class SuggestionData
    {
        public string Text { get; init; } = string.Empty;

        public string Source { get; init; } = string.Empty;

        public int Frequency { get; set; }
    }
}
