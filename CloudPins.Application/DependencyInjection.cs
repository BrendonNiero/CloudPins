using CloudPins.Application.Boards.Create;
using CloudPins.Application.Boards.GetAll;
using CloudPins.Application.Boards.GetById;
using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using CloudPins.Application.Tags.Create;
using CloudPins.Application.Tags.GetAll;
using Microsoft.Extensions.DependencyInjection;

namespace CloudPins.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {

        // BOARDS
        services.AddScoped<CreateBoardCommandHandler>();
        services.AddScoped<GetBoardByIdQueryHandler>();
        services.AddScoped<GetAllBoardsQueryHandler>();

        // PINS
        services.AddScoped<CreatePinCommandHandler>();
        services.AddScoped<GetPinByIdQueryHandler>();
        services.AddScoped<GetPinsFeedQueryHandler>();

        // TAGS
        services.AddScoped<CreateTagCommandHandler>();
        services.AddScoped<GetAllTagsQueryHanddler>();

        return services;
    }
}