using CloudPins.Domain.Pins;
using CloudPins.Domain.Tests.Builders;

namespace CloudPins.Domain.Tests.Pins;

public class PinTest
{
    [Fact]
    public void Create_Should_Create_Pin_When_Data_Is_Valid()
    {
        // Arrange
        var tag1 = Guid.NewGuid();
        var tag2 = Guid.NewGuid();

        // Act
        var pin = new PinBuilder().WithTags(tag1, tag2).Build();

        // Assert
        Assert.NotNull(pin);
        Assert.Equal(2, pin.PinTags.Count());
        Assert.Contains(pin.PinTags, t => t.TagId == tag1);
        Assert.Contains(pin.PinTags, t => t.TagId == tag2);
    }

    [Fact]
    public void Create_Should_Not_Add_Duplicate_Tags()
    {
        // Arrange
        var tagId = Guid.NewGuid();
        // Act
        var pin = new PinBuilder().WithTags(tagId, tagId, tagId).Build();
        // Assert
        Assert.Single(pin.PinTags);
    }

    [Fact]
    public void AddTag_Should_Not_Add_Tag_When_Already_Exists()
    {
        // Arrange
        var tagId = Guid.NewGuid();

        // Act
        var pin = new PinBuilder().WithTags(tagId).Build();

        pin.AddTag(tagId);

        // Assert
        Assert.Single(pin.PinTags);
    }

    [Fact]
    public void AddLike_Should_Increment_Likes()
    {
        // Act
        var pin = new PinBuilder().Build();
        pin.AddLike();

        // Assert
        Assert.Equal(1, pin.LikesCount);
    }

    [Fact]
    public void RemoveLike_Should_Not_Go_Below_Zero()
    {
        // Act
        var pin = new PinBuilder().Build();

        pin.RemoveLike();

        // Assert
        Assert.Equal(0, pin.LikesCount);
    }

    [Fact]
    public void Create_Should_Throw_When_Title_Is_Empty()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => 
            new PinBuilder().WithTitle("").Build());
    }

    [Fact]
    public void Create_Should_Throw_When_ImageUrl_Is_Empty()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => 
            new PinBuilder().WithImageUrl("").Build());
    }
    [Fact]
    public void Create_Should_Throw_When_ThumbnailUrl_Is_Empty()
    {
        // Assert
        Assert.Throws<ArgumentException>(() => 
            new PinBuilder().WithThumbnailUrl("").Build());
    }

    [Fact]
    public void Create_Should_Throw_When_OwnerId_Is_Empty()
    {
        // Assert
        Assert.Throws<ArgumentException>(() =>
            new PinBuilder().WithOwner(Guid.Empty).Build());
    }

    [Fact]
    public void Create_Should_Throw_When_BoardId_Is_Empty()
    {
        // Assert
        Assert.Throws<ArgumentException>(() =>
            new PinBuilder().WithBoard(Guid.Empty).Build());
    }
}