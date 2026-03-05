using CloudPins.Domain.Tags;

namespace CloudPins.Domain.Tests.Tags;

public class TagsTest
{
    [Fact]
    public void Constructor_Should_Create_Tag_When_Data_Is_Valid()
    {
        // Act
        var tag = new Tag("my tagname");

        // Assert
        Assert.Equal("my tagname", tag.Name);
        Assert.NotEqual(Guid.Empty, tag.Id);
    }

    [Fact]
    public void Constructor_Should_Throw_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => 
            new Tag(""));
    }
}