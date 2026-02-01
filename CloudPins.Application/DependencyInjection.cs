using CloudPins.Application.Boards.Create;
using CloudPins.Application.Boards.GetAll;
using CloudPins.Application.Boards.GetById;
using CloudPins.Application.Pins.Create;
using CloudPins.Application.Pins.GetById;
using CloudPins.Application.Pins.GetFeed;
using CloudPins.Application.Pins.LikePin;
using CloudPins.Application.Pins.UnlikePin;
using CloudPins.Application.Tags.Create;
using CloudPins.Application.Tags.GetAll;
using CloudPins.Application.Users.Create;
using CloudPins.Application.Users.Login;
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
        services.AddScoped<GetFeedByPinQueryHandler>();

        // TAGS
        services.AddScoped<CreateTagCommandHandler>();
        services.AddScoped<GetAllTagsQueryHanddler>();

        // LIKES
        services.AddScoped<LikePinCommandHandler>();
        services.AddScoped<UnlikePinCommandHandler>();

        // USERS 
        services.AddScoped<CreateUserCommandHandler>();
        services.AddScoped<LoginCommandHandler>();

        return services;
    }
}