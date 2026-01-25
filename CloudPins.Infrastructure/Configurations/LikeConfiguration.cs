using CloudPins.Domain.Likes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CloudPins.Infrastructure.Configurations;
public class LikeConfiguration : IEntityTypeConfiguration<Like>
{
    public void Configure(EntityTypeBuilder<Like> builder)
    {
        builder.ToTable("Likes");

        builder.HasKey(l => new { l.UserId, l.PinId });

        builder.Property(l => l.CreatedAt).IsRequired();
    }
}