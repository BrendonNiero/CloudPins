using CloudPins.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CloudPins.Infrastructure.Extensions;

public static class MigrationExtensions
{
    public static async Task ApplyMigrationAsync(this IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<CloudPinsDbContext>();

        await dbContext.Database.MigrateAsync();
    }
}