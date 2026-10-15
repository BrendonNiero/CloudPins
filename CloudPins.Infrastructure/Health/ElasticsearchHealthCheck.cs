using CloudPins.Infrastructure.Search;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CloudPins.Infrastructure.Health;

public sealed class ElasticsearchHealthCheck : IHealthCheck
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchOptions _options;

    public ElasticsearchHealthCheck(
        ElasticsearchClient client,
        Microsoft.Extensions.Options.IOptions<ElasticsearchOptions> options)
    {
        _client = client;
        _options = options.Value;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var response = await _client.Indices.ExistsAsync(
                _options.IndexName,
                cancellationToken);

            return response.Exists
                ? HealthCheckResult.Healthy("Elasticsearch is available.")
                : HealthCheckResult.Unhealthy(
                    $"Elasticsearch index '{_options.IndexName}' is unavailable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "Elasticsearch health check failed.",
                exception);
        }
    }
}
