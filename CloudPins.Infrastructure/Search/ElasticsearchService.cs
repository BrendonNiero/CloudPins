using CloudPins.Application.Common.Interfaces;
using CloudPins.Application.Pins.Search;
using CloudPins.Domain.Pins;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Options;

namespace CloudPins.Infrastructure.Search;

public sealed class ElasticsearchService : IPinSearchService
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

    public async Task<PinSearchResult> SearchAsync(
        string searchTerm,
        int page = 1,
        int pageSize = 20,
        CancellationToken cancellationToken = default)
    {
        page = Math.Max(page, 1);
        pageSize = Math.Clamp(pageSize, 1, 100);

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return new PinSearchResult
            {
                Page = page,
                PageSize = pageSize,
                Total = 0
            };
        }

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

        var items = response.Documents
            .Select(document => new PinSearchItem
            {
                Id = document.Id,
                ThumbnailUrl = document.ThumbnailUrl,
                ImageUrl = document.ImageUrl,
                Title = document.Title,
                Description = document.Description,
                Tags = document.Tags
            })
            .ToArray();

        return new PinSearchResult
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = response.Total
        };
    }

    public async Task IndexAsync(
        Pin pin,
        IEnumerable<string> tagNames,
        CancellationToken cancellationToken = default)
    {
        var document = PinDocumentMapper.ToDocument(pin, tagNames);

        var response = await _client.IndexAsync(
            document,
            request => request
                .Index(_options.IndexName)
                .Id(pin.Id.ToString()),
            cancellationToken);

        if (!response.IsValidResponse)
        {
            throw new InvalidOperationException(
                $"Could not index pin '{pin.Id}' in Elasticsearch.");
        }
    }

    public async Task UpdateAsync(
        Pin pin,
        IEnumerable<string> tagNames,
        CancellationToken cancellationToken = default)
    {
        await IndexAsync(pin, tagNames, cancellationToken);
    }

    public async Task DeleteAsync(
        Guid pinId,
        CancellationToken cancellationToken = default)
    {
        var response = await _client.DeleteAsync<PinDocument>(
            pinId.ToString(),
            request => request.Index(_options.IndexName),
            cancellationToken);

        if (!response.IsValidResponse &&
            response.Result != Result.NotFound)
        {
            throw new InvalidOperationException(
                $"Could not delete pin '{pinId}' from Elasticsearch.");
        }
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
                $"Could not create index '{_options.IndexName}'.");
        }
    }
}