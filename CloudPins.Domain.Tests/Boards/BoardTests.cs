using CloudPins.Domain.Boards;
using CloudPins.Domain.Users;

namespace CloudPins.Domain.Tests.Boards;

public class BoardTests
{
    [Fact]
    public void Create_Should_Create_Board_When_Data_Is_Valid()
    {
        // Arrange
        var ownerId = Guid.NewGuid();
        var boardName = "My Board";
        var isPublic = true;

        // Act
        var board = Board.Create(ownerId, boardName, isPublic);

        // Assert
        Assert.NotNull(board);
        Assert.Equal(ownerId, board.OwnerId);
        Assert.Equal(boardName, board.Name);
        Assert.Equal(isPublic, board.IsPublic);
        Assert.NotEqual(Guid.Empty, board.Id);
    }

    [Fact]
    public void ChangeVisibility_Should_Change_Board_Visibility()
    {
        // Arrange
        var board = Board.Create(Guid.NewGuid(), "My Board", true);

        // Act
        board.ChangeVisibility(false);

        // Assert
        Assert.False(board.IsPublic);
    }

    [Fact]
    public void Rename_Should_Rename_Board_Name()
    {
        // Arrange
        var board = Board.Create(Guid.NewGuid(), "My Board", true);

        // Act
        board.Rename("Renamed Board");

        // Assert
        Assert.Equal("Renamed Board", board.Name);
    }
}