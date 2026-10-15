using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace CloudPins.Infrastructure.Observability;

public static class CloudPinsTelemetry
{
    public const string ActivitySourceName = "CloudPins.Infrastructure";
    public const string MeterName = "CloudPins.Infrastructure";

    public static readonly ActivitySource ActivitySource =
        new(ActivitySourceName);

    public static readonly Meter Meter = new(MeterName);

    public static readonly Histogram<double> SearchDuration =
        Meter.CreateHistogram<double>(
            "cloudpins.search.duration",
            unit: "ms");

    public static readonly Counter<long> PinsIndexed =
        Meter.CreateCounter<long>("cloudpins.pins.indexed");

    public static readonly Counter<long> SearchFailures =
        Meter.CreateCounter<long>("cloudpins.search.failures");

    public static readonly Counter<long> SearchFallbacks =
        Meter.CreateCounter<long>("cloudpins.search.fallbacks");

    public static readonly Counter<long> SuggestionFailures =
        Meter.CreateCounter<long>("cloudpins.suggestions.failures");
}
