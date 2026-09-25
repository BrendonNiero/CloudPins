namespace CloudPins.Infrastructure.Search;

public sealed class ElasticsearchOptions
{
    public string Url { get; init; } = "http://localhost:9200";
    public string IndexName { get; init; } = "cloudpins-pins";
}
