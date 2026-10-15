using CloudPins.Infrastructure.Persistence;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace CloudPins.Infrastructure.Health;

public sealed class PostgresHealthCheck : IHealthCheck
{
    private readonly CloudPinsDbContext _dbContext;

    public PostgresHealthCheck(CloudPinsDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var connected = await _dbContext.Database
                .CanConnectAsync(cancellationToken);

            return connected
                ? HealthCheckResult.Healthy("PostgreSQL is available.")
                : HealthCheckResult.Unhealthy("PostgreSQL is unavailable.");
        }
        catch (Exception exception)
        {
            return HealthCheckResult.Unhealthy(
                "PostgreSQL health check failed.",
                exception);
        }
    }
}
