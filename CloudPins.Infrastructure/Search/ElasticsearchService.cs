using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace CloudPins.Infrastructure.Search;

public sealed class ElasticsearchService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchOptions _options;

    public ElasticsearchService(
        ElasticsearchClient client,
        IOptions<ElasticsearchOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<bool> IsAvailableAsync(
        CancellationToken cancellationToken = default)
    {
        var response = await _client.PingAsync(cancellationToken);

        return response.IsValidResponse;
    }

    public async Task EnsureIndexAsync(
        CancellationToken cancellationToken = default)
    {
        var existsResponse = await _client.Indices.ExistsAsync(
            _options.IndexName,
            cancellationToken);

        if (existsResponse.Exists)
            return;

        var createResponse = await _client.Indices.CreateAsync(
            _options.IndexName,
            cancellationToken: cancellationToken);

        if (!createResponse.IsValidResponse)
        {
            throw new InvalidOperationException(
                $"Could not create Elasticsearch index '{_options.IndexName}'.");
        }
    }

    public async Task IndexAsync<TDocument>(
        string id,
        TDocument document,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.IndexAsync(
            document,
            request => request
                .Index(_options.IndexName)
                .Id(id),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException(
                $"Could not index document '{id}'.");
        }
    }

    public async Task<IReadOnlyCollection<PinDocument>> SearchPinsAsync(
        string searchTerm,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return Array.Empty<PinDocument>();

        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        var from = (page - 1) * pageSize;

        var response = await _client.SearchAsync<PinDocument>(
            request => request
                .Index(_options.IndexName)
                .From(from)
                .Size(pageSize)
                .Query(query => query
                    .MultiMatch(multiMatch => multiMatch
                        .Query(searchTerm)
                        .Fields(new[]
                        {
                            "title^3",
                            "description^2",
                            "tags"
                        }))),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException(
                "Could not search pins in Elasticsearch.");
        }

        return response.Documents;
    }

    public async Task UpdateAsync<TDocument>(
        string id,
        TDocument document,
        CancellationToken cancellationToken = default)
    {
        await IndexAsync(id, document, cancellationToken);
    }

    public async Task DeleteAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.DeleteAsync<PinDocument>(
            id,
            request => request.Index(_options.IndexName),
            cancellationToken);

        if (!response.IsValidResponse &&
            response.Result != Result.NotFound)
        {
            throw new InvalidOperationException(
                $"Could not delete document '{id}' from Elasticsearch.");
        }
    }
}