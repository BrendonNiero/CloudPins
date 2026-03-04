using CloudPins.Domain.Users;

namespace CloudPins.Domain.Testes.Users;

public class UserTests
{
    [Fact]
    public void Create_Should_Create_User_When_Data_Is_Valid()
    {
        // Arrange
        var name = "Brendon";
        var email = "brendon@gmail.com";
        var profileUrl = "/storage/profiles/profile.png";
        var passwordHash = "my_hashed_pass";

        // Act
        var user = User.Create(name, profileUrl, email, passwordHash);

        // Assert
        Assert.NotNull(user);
        Assert.Equal(name, user.Name);
        Assert.Equal(email.ToLowerInvariant(), user.Email);
        Assert.Equal(profileUrl, user.ProfileUrl);
        Assert.Equal(passwordHash, user.PasswordHash);
        Assert.NotEqual(Guid.Empty, user.Id);
    }

    [Fact]
    public void Create_Should_Throw_When_Name_Is_Empty()
    {
        Assert.Throws<ArgumentException>(() => 
            User.Create("", "url", "email@gmail.com", "pass_hash"));
    }

    [Fact]
    public void CheckPassword_Should_Return_True_When_Verify_Is_Valid()
    {
        // Arrange
        var user = User.Create("Name", "url", "email@gmail.com", "pass_hash");

        bool FakeVerify(string input, string stored)
            => input == "123" && stored == "pass_hash";

        // Act
        var result = user.CheckPassword("123", FakeVerify);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void UpdateProfile_Should_Update_Name_And_ProfileUrl()
    {
        // Arrange
        var user = User.Create("Old Name", "oldUrl", "email@gmail.com", "pass_hash");

        // Act
        user.UpdateProfile("New Name", "newUrl");

        // Assert

        Assert.Equal("New Name", user.Name);
        Assert.Equal("newUrl", user.ProfileUrl);
    }
}