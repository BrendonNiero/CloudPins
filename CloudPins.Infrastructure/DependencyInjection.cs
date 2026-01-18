using CloudPins.Application.Common.Interfaces;
using CloudPins.Infrastructure.Persistence;
using CloudPins.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CloudPins.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration
    )
    {
        services.AddDbContext<CloudPinsDbContext>(options =>
            options.UseNpgsql(
                configuration.GetConnectionString("DefaultConnection")
            ));

        services.AddScoped<IPinRepository, PinRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        services.AddScoped<IPinReadRepository, PinReadRepository>();
        services.AddScoped<IBoardReadRepository, BoardReadRepository>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ITagReadRepository, TagReadRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}