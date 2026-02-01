using CloudPins.Domain.Boards;
using CloudPins.Domain.Likes;
using CloudPins.Domain.Pins;
using CloudPins.Domain.Tags;
using CloudPins.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace CloudPins.Infrastructure.Persistence;

public class CloudPinsDbContext : DbContext
{
    public CloudPinsDbContext(DbContextOptions<CloudPinsDbContext>options)
        : base(options)
    {    
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Board> Boards => Set<Board>();
    public DbSet<Pin> Pins => Set<Pin>();
    public DbSet<Tag> Tags => Set<Tag>();
    public DbSet<Like> Likes => Set<Like>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(CloudPinsDbContext).Assembly
        );

        modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Board>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Pin>().HasQueryFilter(x => !x.IsDeleted);
        modelBuilder.Entity<Tag>().HasQueryFilter(x => !x.IsDeleted);

        modelBuilder.Entity<PinTag>(builder =>
        {
            builder.HasKey(pt => new { pt.PinId, pt.TagId });

            builder.HasOne<Pin>()
                .WithMany(p => p.PinTags)
                .HasForeignKey(pt => pt.PinId);

            builder.HasOne<Tag>()
                .WithMany()
                .HasForeignKey(pt => pt.TagId);
        });
    }
}