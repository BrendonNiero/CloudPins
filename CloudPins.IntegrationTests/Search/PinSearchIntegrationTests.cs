using System.Net.Http.Json;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace CloudPins.IntegrationTests.Search;

public sealed class PinSearchIntegrationTests
{
    private readonly HttpClient _apiClient;
    private readonly HttpClient _elasticsearchClient;

    public PinSearchIntegrationTests()
    {
        _apiClient = new HttpClient
        {
          BaseAddress = new Uri(
            Environment.GetEnvironmentVariable("CLOUDPINS_API_URL") ?? "http://localhost:5023"
          )
        };

        _elasticsearchClient = new HttpClient
        {
            BaseAddress = new Uri(
                Environment.GetEnvironmentVariable("ELASTICSEARCH_URL") ?? "http://localhost:9200"
            )
        };
    }

    [Fact]
    public async Task Elasticsearch_should_contain_seeded_pins()
    {
        var response = await _elasticsearchClient.GetAsync(
            "/cloudpins_documents/_count"
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<CountResponse>();

        Assert.NotNull(result);
        Assert.True(result!.count > 0);
    }

    private sealed class CountResponse
    {
        [JsonPropertyName("count")]
        public int count { get; init; }
    }
}