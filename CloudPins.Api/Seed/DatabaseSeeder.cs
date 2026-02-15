using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;
using CloudPins.Domain.Pins;
using CloudPins.Domain.Tags;
using CloudPins.Domain.Users;
using CloudPins.Infrastructure.Persistence;

namespace CloudPins.Api.Seed;
public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<CloudPinsDbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
        var passwordHahser = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if(context.Users.Any()) return;

        // CRIAR USUÁRIO
        var passwordHasher = passwordHahser.Hash("123");

        var user = User.Create(
            "Brendon Berzins",
            "/uploads/profile/user1.jpg",
            "admin@gmail.com",
            passwordHasher
        );

        context.Users.Add(user);
        await context.SaveChangesAsync();

        // CRIAR BOARD
        var board = Board.Create(
            user.Id,
            "Seed Boards Admin",
            true
        );

        context.Boards.Add(board);
        await context.SaveChangesAsync();

        // CRAIR TAGS
        var tagAnime = new Tag("anime");
        var tagGames = new Tag("games");

        context.Tags.AddRange(tagAnime, tagGames);
        await context.SaveChangesAsync();

        // CRIAR PINS
        var imagesPath = Path.Combine(
            AppContext.BaseDirectory,
            "Seed",
            "Images"
        );

        var files = Directory.GetFiles(imagesPath);

        foreach(var file in files)
        {
            var bytes = await File.ReadAllBytesAsync(file);
            var extension = Path.GetExtension(file);

            var contentType = extension switch
            {
                ".jpeg" => "image/jpeg",
                ".jpg" => "image/jpg",
                ".png" => "image/png",
                _ => "image/jpeg"
            };

            var imageUrl = await storage.UploadAsync(
                bytes,
                contentType,
                CancellationToken.None
            );

            var pin = Pin.Create(
                user.Id,
                board.Id,
                imageUrl,
                imageUrl,
                Path.GetFileNameWithoutExtension(file),
                "Imagem criada por Seed",
                []
            );

            context.Pins.Add(pin);
        }

        await context.SaveChangesAsync();
    }
}