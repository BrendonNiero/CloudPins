using CloudPins.Application.Common.Interfaces;
using CloudPins.Domain.Boards;
using CloudPins.Domain.Pins;
using CloudPins.Domain.Tags;
using CloudPins.Domain.Users;
using CloudPins.Infrastructure.Persistence;
using CloudPins.Infrastructure.Search;

namespace CloudPins.Api.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var elasticsearchService =
            scope.ServiceProvider.GetRequiredService<ElasticsearchService>();
        var context = scope.ServiceProvider.GetRequiredService<CloudPinsDbContext>();
        var storage = scope.ServiceProvider.GetRequiredService<IStorageService>();
        var passwordHasher = scope.ServiceProvider.GetRequiredService<IPasswordHasher>();

        if (context.Users.Any())
            return;

        var imagesPath = Path.Combine(AppContext.BaseDirectory, "Seed", "Images");
        if (!Directory.Exists(imagesPath))
            throw new DirectoryNotFoundException($"Seed images directory was not found: {imagesPath}");

        var profileUrl = await UploadImageAsync(Path.Combine(imagesPath, "user1.jpg"), storage);
        var user = User.Create("Brendon Berzins", profileUrl, "admin@gmail.com", passwordHasher.Hash("123"));
        context.Users.Add(user);
        await context.SaveChangesAsync();

        var board = Board.Create(user.Id, "Seed Boards Admin", true);
        context.Boards.Add(board);

        var tagNames = new[]
        {
            "anime", "car", "games", "room", "setup", "food", "sport", "outfit", "woman",
            "aesthetic", "gaming", "home-office", "interior-design", "formula-1", "red-bull",
            "motorcycle", "ski", "jet-ski", "breakfast", "ramen"
        };
        var tags = tagNames.ToDictionary(name => name, name => new Tag(name));
        context.Tags.AddRange(tags.Values);
        await context.SaveChangesAsync();

        var createdPins = new List<(Pin Pin, string[] Tags)>();

        foreach (var seedPin in BuildSeedPins())
        {
            var imagePath = Path.Combine(imagesPath, seedPin.FileName);

            if (!File.Exists(imagePath))
                continue;

            var imageUrl = await UploadImageAsync(imagePath, storage);

            var tagIds = seedPin.Tags
                .Where(tags.ContainsKey)
                .Select(tagName => tags[tagName].Id)
                .ToArray();

            var pin = Pin.Create(
                user.Id,
                board.Id,
                imageUrl,
                imageUrl,
                seedPin.Title,
                seedPin.Description,
                tagIds);

            context.Pins.Add(pin);
            createdPins.Add((pin, seedPin.Tags));
        }

        await context.SaveChangesAsync();
        foreach (var createdPin in createdPins)
        {
            await elasticsearchService.IndexAsync(
                createdPin.Pin,
                createdPin.Tags);
        }
    }

    private static async Task<string> UploadImageAsync(string imagePath, IStorageService storage)
    {
        if (!File.Exists(imagePath))
            return string.Empty;

        var bytes = await File.ReadAllBytesAsync(imagePath);
        return await storage.UploadAsync(bytes, GetContentType(imagePath), CancellationToken.None);
    }

    private static string GetContentType(string filePath) => Path.GetExtension(filePath).ToLowerInvariant() switch
    {
        ".jpg" or ".jpeg" => "image/jpeg",
        ".png" => "image/png",
        ".webp" => "image/webp",
        _ => "application/octet-stream"
    };

    private static IEnumerable<SeedPin> BuildSeedPins()
    {
        var manualPins = new[]
        {
            new SeedPin("anime1.jpg", "Purple anime icon Satoru Gojo", "Satoru Gojo Jujutsu Kaisen profile anime picture with background purple", ["anime"]),
            new SeedPin("anime2.jpg", "Qin Shi Huang profile picture icon", "Record of Ragnarok profile picture anime", ["anime"]),
            new SeedPin("anime3.jpg", "Purple anime profile picture", "Purple anime profile picture.", ["anime"]),
            new SeedPin("anime4.jpg", "Girl Colors AI anime profile picture.", "Color AI girl profile picture", ["anime"]),
            new SeedPin("anime5.jpg", "Purple girl anime profile picture.", "Purple AI anime profile picture.", ["anime"]),
            new SeedPin("anime6.jpg", "Girl Colors AI anime profile picture.", "Color AI girl profile picture", ["anime"]),
            new SeedPin("anime7.jpg", "Pink AI anime 3D profile picture.", "AI pink girl profile picture 3d", ["anime"]),
            new SeedPin("anime8.jpg", "Pink anime girl with a cat", "Profile picture cat girl with pink haircut holding a cat.", ["anime"]),
            new SeedPin("anime9.jpg", "Girl with white hair holding a cat", "Girl with white hair and red eyes holding black cat with red eyes.", ["anime"]),
            new SeedPin("anime10.jpg", "Girl with black hair looking up", "Girl with black hair and red eyes looking up", ["anime"]),
            new SeedPin("car1.jpg", "BMW M4 Coup", "BMW m4 coup F82 generation manufactured in 2014.", ["car"]),
            new SeedPin("car2.jpg", "BMW M4 Coup", "F82 generation. White BMW beautiful car.", ["car"]),
            new SeedPin("car3.jpg", "Purple Toyota 86 modified", "Purple modified Toyota GT86. Beautifil car.", ["car"]),
            new SeedPin("car4.jpg", "McLaren P1 GTR", "Purple McLaren P1 GTR. an ultra-exclusive hypercar. Aesthetic photo.", ["car"]),
            new SeedPin("car5.jpg", "Nissan Silvia S15", "A red Nissan Silvia S15 aesthetic car.", ["car"]),
            new SeedPin("car6.jpg", "Lamborghini Veneno Roadster", "White aesthetic rare lamborghini veneno roadster.", ["car"]),
            new SeedPin("car7.jpg", "Ferrari F8 Tributo", "Gray Ferrari F8 Tributo italian supercar. Aesthetic photo.", ["car"]),
            new SeedPin("car8.jpg", "Lamborghini Veneno Coupe", "A golden customized chrome lamborghini veneno coupe.", ["car"]),
            new SeedPin("car9.jpg", "AI car generated", "AI-generated car. beautifully design vehicle deco-inpired concept roadster.", ["car"]),
            new SeedPin("car10.jpg", "McLaren 600Lt Coupe", "A white beautifully vehicle McLaren 600LT coupe. High-performance soports.", ["car"]),
            new SeedPin("games1.jpg", "Vergil Devil May Cry Profile picture", "Aesthetic image of Verfil from Devil May Cry.", ["games"]),
            new SeedPin("games2.jpg", "Dante Devil May Cry", "Aesthetic profile picture of Dante from Devil May Cry 5.", ["games"]),
            new SeedPin("games3.jpg", "Eda Resident Evil 4 Remake", "Aesthetic profile picture of Eda from Resident Evil 4 Remake.", ["games"]),
            new SeedPin("games4.jpg", "Leon S. Kennedy Resident Evil 4 Remake", "Aesthetic profile picture of Leon S. Kennedy from Resident Evil 4 Remake holding a weapon.", ["games"]),
            new SeedPin("games5.jpg", "Leon S. Kennedy Resident Evil 4 Remake", "Aesthetic profile picture of Leon S. Kennedy from Resident Evil 4 Remake looking foward.", ["games"]),
            new SeedPin("games6.jpg", "Elie the last of us PART II", "Aesthetic profile picture of Elie from The last of US 2 looking up.", ["games"]),
            new SeedPin("games7.jpg", "Funny posing of Elie TLOU 2", "Aesthetic profile picture of Elie from The last of US 2 posing.", ["games"]),
            new SeedPin("games8.jpg", "Joel Millerthe last of us 1", "Aesthetic profile picture of Joel Miller from The last of US.", ["games"]),
            new SeedPin("games9.jpg", "Malenia, Blade of Miquella", "Elden Ring. Malenia, aesthetic profile picture.", ["games"]),
            new SeedPin("games10.jpg", "Ashley Graham Resident Evil 4 Remake", "Aesthetic profile picture of Ashley Graham smiling from Resident Evil 4 Remake looking foward.", ["games"]),
            new SeedPin("games11.jpg", "Maelle Clair Obscur Expedition 33", "Aesthetic portrait of Maelle from Clair Obscur: Expedition 33, featuring a dramatic fantasy atmosphere and elegant character design.", ["games"]),
            new SeedPin("games12.jpg", "Sciel Clair Obscur Expedition 33", "Aesthetic portrait of Sciel from Clair Obscur: Expedition 33, with a mysterious visual style and detailed fantasy character design.", ["games"]),
            new SeedPin("games13.jpg", "Lune Clair Obscur Expedition 33", "Aesthetic portrait of Lune from Clair Obscur: Expedition 33, combining magical elements, expressive colors and a cinematic mood.", ["games"]),
            new SeedPin("games14.jpg", "GTA 6 OLED TV Aesthetic", "Grand Theft Auto VI displayed on an OLED television, creating a cinematic gaming setup with rich colors and a modern aesthetic.", ["games", "gaming", "aesthetic"]),
            new SeedPin("games15.jpg", "GTA 6 PS5 OLED Gaming Setup", "Aesthetic PlayStation 5 gaming setup showing Grand Theft Auto VI on an OLED TV, with immersive lighting and a clean entertainment space.", ["games", "gaming", "aesthetic"]),
            new SeedPin("food1.jpg", "Ramen Anime Aesthetic", "A warm bowl of ramen presented with anime-inspired details, creating a cozy Japanese food and aesthetic scene.", ["food", "ramen", "anime", "aesthetic"]),
            new SeedPin("food2.jpg", "Fresh Fruit Salad Aesthetic", "Colorful fresh fruit salad arranged in an aesthetic composition, ideal for a light, healthy and refreshing meal.", ["food", "aesthetic"]),
            new SeedPin("food3.jpg", "Open-Faced Sandwiches and Iced Tea", "Light breakfast or lunch setup with two open-faced sandwiches and a refreshing glass of iced tea.", ["food", "breakfast"]),
            new SeedPin("food4.jpg", "Caprese Sandwiches and Coffee", "Two open-faced caprese sandwiches served with a cup of milky coffee in a cozy aesthetic breakfast setup.", ["food", "breakfast"]),
            new SeedPin("food5.jpg", "Folded Crepe with Black Coffee", "Golden-brown folded crepe or thin omelet served with a hot cup of black coffee topped with a light foam layer.", ["food", "breakfast"]),
            new SeedPin("sport1.jpg", "Red Bull Formula 1 Racing Woman", "Woman wearing a Red Bull shirt at a Formula 1 racetrack, capturing the energy and atmosphere of motorsport.", ["sport", "formula-1", "red-bull", "woman"]),
            new SeedPin("sport2.jpg", "Red Bull Motorcycle Trail in the Sky", "Red Bull motorcycle rider performing an impressive trail stunt high in the sky, creating an energetic action sports scene.", ["sport", "red-bull", "motorcycle"]),
            new SeedPin("sport3.jpg", "Red Motorcycle Trail Aesthetic", "Aesthetic action photograph of a red motorcycle rider performing a trail stunt with a woman watching the scene.", ["sport", "motorcycle", "woman", "aesthetic"]),
            new SeedPin("sport4.jpg", "Skiing in a Snowy Landscape", "Aesthetic winter sports photograph of a man skiing through a snowy mountain landscape surrounded by soft white scenery.", ["sport", "ski", "aesthetic"]),
            new SeedPin("sport5.jpg", "Jet Ski Adventure in the Ocean", "Aesthetic jet ski scene on the open ocean, capturing movement, blue water and the freedom of water sports.", ["sport", "jet-ski", "aesthetic"]),
            new SeedPin("room1.jpg", "Dreamy Whimsigoth Bedroom Inspo ✨💜", "Creating the ultimate cozy sanctuary with purple LED lighting, vinyl record wall decor, and hanging vines. Perfect aesthetic room inspiration for anyone loving dark, moody, and comfortable room layouts. Save this for your next bedroom makeover! 🕸️🐈‍⬛", ["room"]),
            new SeedPin("room2.jpg", "Architectural & Spatial Layout", "elevated wooden platform to isolate the sleeping quarters from the main lounge area.", ["room"]),
            new SeedPin("room4.jpg", "Dark Aesthetic Kitchen with Purple LED Lights 💜✨", "Giving major cozy, modern vibes with this gorgeous kitchen setup. Love how the matte black cabinets and white marble countertops pop under the purple ambient LED under-cabinet lighting. Perfect kitchen inspiration for a moody, high-contrast interior design. Save this pin for your next home remodel! 🍇💻", ["room"])
        };

        var categories = new[]
        {
            new SeedCategory("anime", "Anime profile picture", "Aesthetic anime character portrait with expressive colors, suitable for a profile picture.", ["anime", "aesthetic"]),
            new SeedCategory("car", "Aesthetic sports car", "Automotive inspiration featuring a high-performance sports car, detailed design and striking colors.", ["car", "aesthetic"]),
            new SeedCategory("games", "Video game character", "Gaming profile picture featuring a memorable video game character and an atmospheric visual style.", ["games", "gaming"]),
            new SeedCategory("room", "Bedroom and interior inspiration", "Interior design inspiration with a cozy room layout, decorative details and ambient lighting.", ["room", "interior-design", "aesthetic"]),
            new SeedCategory("setup", "Modern desk setup", "Workspace inspiration with a clean desk, technology, ambient lighting and an organized home office.", ["setup", "home-office", "gaming"])
        };

        var fallbackPins = categories
            .SelectMany(category => Enumerable.Range(1, 10).Select(number => new SeedPin(
                $"{category.Prefix}{number}.jpg", $"{category.Title} {number}", category.Description, category.Tags)))
            .Where(pin => manualPins.All(manualPin => manualPin.FileName != pin.FileName));

        return manualPins.Concat(fallbackPins);
    }

    private sealed record SeedCategory(string Prefix, string Title, string Description, string[] Tags);
    private sealed record SeedPin(string FileName, string Title, string Description, string[] Tags);
}
