using TOON;
using Xunit;

namespace TOON.Tests;

public class ToonSerializerTests
{
    [Fact]
    public void Serialize_SimpleObject_ReturnsValidToon()
    {
        // Arrange
        var user = new TestUser { Id = 1, Name = "Alice" };

        // Act
        var result = ToonSerializer.Serialize(user);

        // Assert
        Assert.Contains("Id", result);
        Assert.Contains("Name", result);
        Assert.Contains("1", result);
        Assert.Contains("Alice", result);
    }

    [Fact]
    public void Serialize_ListOfObjects_ReturnsTabularFormat()
    {
        // Arrange
        var users = new List<TestUser>
        {
            new() { Id = 1, Name = "Alice" },
            new() { Id = 2, Name = "Bob" }
        };

        // Act
        var result = ToonSerializer.Serialize(users);

        // Assert
        Assert.Contains("2", result); // Count
        Assert.Contains("Id", result);
        Assert.Contains("Name", result);
    }

    [Fact]
    public void FromJson_ValidJson_ReturnsValidToon()
    {
        // Arrange
        var json = @"{""id"": 1, ""name"": ""Alice""}";

        // Act
        var result = ToonSerializer.FromJson(json);

        // Assert
        Assert.NotEmpty(result);
        Assert.Contains("id", result);
        Assert.Contains("Alice", result);
    }

    [Fact]
    public void Serialize_WithCamelCase_UsesCorrectNaming()
    {
        // Arrange
        var user = new TestUser { Id = 1, Name = "Alice" };
        var options = new ToonSerializerOptions
        {
            PropertyNamingPolicy = ToonNamingPolicy.CamelCase
        };

        // Act
        var result = ToonSerializer.Serialize(user, options);

        // Assert
        Assert.Contains("id", result);
        Assert.Contains("name", result);
    }

    [Fact]
    public void Serialize_WithToonIgnore_IgnoresProperty()
    {
        // Arrange
        var user = new TestUserWithIgnore { Id = 1, Name = "Alice", Secret = "hidden" };

        // Act
        var result = ToonSerializer.Serialize(user);

        // Assert
        Assert.DoesNotContain("Secret", result);
        Assert.DoesNotContain("hidden", result);
    }
}

public class TestUser
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}

public class TestUserWithIgnore
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    [ToonIgnore]
    public string Secret { get; set; } = string.Empty;
}
