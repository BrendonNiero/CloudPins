using System.Net.Http.Headers;
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

    [Fact]
    public async Task Search_should_return_matching_pins()
    {
        await AuthenticateAsync();

        var response = await _apiClient.GetAsync(
            "/search/car?page=1&pageSize=20"
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<PinSearchResponse>();

        Assert.NotNull(result);
        Assert.NotNull(result!.Items);
        Assert.True(result.Total > 0);
        Assert.True(result.Items.Count > 0);
        Assert.True(result.Page == 1);
        Assert.True(result.PageSize == 20);
    }

    [Fact]
    public async Task Search_should_return_empty_items_for_unknown_term()
    {
        await AuthenticateAsync();

        var response = await _apiClient.GetAsync(
            "/search/term-that-does-not-exits?page=1&pageSize=20"
        );

        response.EnsureSuccessStatusCode();

        var result = await response.Content
            .ReadFromJsonAsync<PinSearchResponse>();

        Assert.NotNull(result);
        Assert.Empty(result!.Items);
        Assert.Equal(0, result.Total);
    }

    private async Task AuthenticateAsync()
    {
        var loginResponse = await _apiClient.PostAsJsonAsync(
            "/auth/login",
            new
            {
                email = "admin@gmail.com",
                password = "123"
            }
        );

        loginResponse.EnsureSuccessStatusCode();

        var login = await loginResponse.Content
            .ReadFromJsonAsync<LoginResponse>();

        Assert.NotNull(login);
        Assert.False(string.IsNullOrWhiteSpace(login!.Token));

        _apiClient.DefaultRequestHeaders.Authorization = 
            new AuthenticationHeaderValue(
                "Bearer",
                login.Token
            );
    }

    private sealed class LoginResponse
    {
        [JsonPropertyName("token")]
        public string Token { get; init; } = string.Empty;
    }

    private sealed class PinSearchResponse
    {
        [JsonPropertyName("items")]
        public List<PinSearchItem> Items { get; init; } = [];
        [JsonPropertyName("page")]
        public int Page { get; init; }
        [JsonPropertyName("pageSize")]
        public int PageSize { get; init; }
        [JsonPropertyName("total")]
        public long Total { get; init; }
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; init; }
    }

    private sealed class PinSearchItem
    {
        [JsonPropertyName("id")]
        public Guid Id { get; init; }
        [JsonPropertyName("thumbnailUrl")]
        public string ThumbnailUrl { get; init; } = string.Empty;
    }

    private sealed class CountResponse
    {
        [JsonPropertyName("count")]
        public int count { get; init; }
    }
}