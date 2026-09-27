using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace CloudPins.Infrastructure.Search;

public sealed class ElasticsearchService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchOptions _options;

    public ElasticsearchService(ElasticsearchClient client, IOptions<ElasticsearchOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<bool> IsAvailableAsync(CancellationToken cancellationToken = default)
    {
        var response = await _client.PingAsync(cancellationToken);
        return response.IsValidResponse;
    }

    public async Task EnsureIndexAsync(CancellationToken cancellationToken = default)
    {
        var existsResponse = await _client.Indices.ExistsAsync(_options.IndexName, cancellationToken);
        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(_options.IndexName, cancellationToken: cancellationToken);
        if (!createResponse.IsValidResponse)
            throw new InvalidOperationException($"Could not create Elasticsearch index '{_options.IndexName}'.");
    }

    public async Task IndexAsync<TDocument>(string id, TDocument document, CancellationToken cancellationToken = default)
    {
        var response = await _client.IndexAsync(document, request => request.Index(_options.IndexName).Id(id), cancellationToken);
        if (!response.IsValidResponse)
            throw new InvalidOperationException($"Could not index document '{id}' in '{_options.IndexName}'.");
    }

    public async Task<IReadOnlyCollection<PinDocument>> SearchPinAsync(
        string searchTerm,
        int size = 20,
        CancellationToken ct = default
    )
    {
        if(string.IsNullOrWhiteSpace(searchTerm))
            return Array.Empty<PinDocument>();

        size = Math.Clamp(size, 1, 100);

        var response = await _client.SearchAsync<PinDocument>(
            request => request
                .Index(_options.IndexName)
                .Size(size)
                .Query(query => query
                    .MultiMatch(multiMatch => multiMatch
                        .Query(searchTerm)
                        .Fields(new[]
                        {
                            "title",
                            "description",
                            "tags"
                        }))),
            ct);

        if(!response.IsValidResponse)
            throw new InvalidOperationException("Could not search pins in Elasticsearch");

        return response.Documents;
    }
}
