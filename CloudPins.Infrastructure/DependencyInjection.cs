using Amazon.S3;
using CloudPins.Application.Common.Interfaces;
using CloudPins.Infrastructure.Persistence;
using CloudPins.Infrastructure.Persistence.Repositories;
using CloudPins.Infrastructure.Security;
using CloudPins.Infrastructure.Storage;
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

        services.Configure<JwtSettings>(
            configuration.GetSection("Jwt")
        );

        services.Configure<StorageOptions>(
            configuration.GetSection("Storage")
        );
        services.AddSingleton<IAmazonS3>(_ =>
        {
            var storage = configuration.GetSection("Storage");
            var config = new AmazonS3Config
            {
                ServiceURL = storage["ServiceUrl"],
                ForcePathStyle = true
            };  
            return new AmazonS3Client(
                storage["AWS_ACCESS_KEY_ID"],
                storage["AWS_SECRET_ACCESS_KEY"],
                config
            );
        });

        services.AddScoped<IStorageService, S3StorageService>();

        services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();

        services.AddScoped<IPinRepository, PinRepository>();
        services.AddScoped<ILikeRepository, LikeRepository>();
        services.AddScoped<IBoardRepository, BoardRepository>();
        services.AddScoped<ITagRepository, TagRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

        services.AddScoped<IPinReadRepository, PinReadRepository>();
        services.AddScoped<IBoardReadRepository, BoardReadRepository>();
        services.AddScoped<IUserReadRepository, UserReadRepository>();
        services.AddScoped<ITagReadRepository, TagReadRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        return services;
    }
}