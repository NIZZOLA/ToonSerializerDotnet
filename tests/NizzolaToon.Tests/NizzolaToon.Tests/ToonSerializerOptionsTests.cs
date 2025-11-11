using Nizzola.ToonConvert;

namespace NizzolaToon.Tests;

public class ToonSerializerOptionsTests
{
    [Fact]
    public void Default_HasCorrectDefaultValues()
    {
        // Arrange & Act
        var options = ToonSerializerOptions.Default;

        // Assert
        Assert.Equal(2, options.IndentSize);
        Assert.True(options.IgnoreNullValues);
        Assert.True(options.FlattenNestedObjects);
        Assert.Equal(64, options.MaxDepth);
        Assert.Null(options.PropertyNamingPolicy);
    }

    [Fact]
    public void Serialize_WithCamelCase_ConvertsPascalToCamelCase()
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
        Assert.Contains("{id,name}:", result);
    }

    [Fact]
    public void Serialize_WithSnakeCase_ConvertsPascalToSnakeCase()
    {
        // Arrange
        var user = new TestUserComplex { UserId = 1, FirstName = "Alice" };
        var options = new ToonSerializerOptions
        {
            PropertyNamingPolicy = ToonNamingPolicy.SnakeCase
        };

        // Act
        var result = ToonSerializer.Serialize(user, options);

        // Assert
        Assert.Contains("{first_name,user_id}:", result);
    }

    [Fact]
    public void Serialize_WithCustomIndentSize_UsesCorrectIndentation()
    {
        // Arrange
        var users = new List<TestUser>
        {
            new() { Id = 1, Name = "Alice" }
        };
        var options = new ToonSerializerOptions
        {
            IndentSize = 4
        };

        // Act
        var result = ToonSerializer.Serialize(users, options);

        // Assert
        var lines = result.Split('\n');
        Assert.StartsWith("    ", lines[1]); // 4 espaços
    }

    [Fact]
    public void Serialize_WithIgnoreNullValuesFalse_IncludesNulls()
    {
        // Arrange
        var user = new TestUser { Id = 1, Name = null };
        var options = new ToonSerializerOptions
        {
            IgnoreNullValues = false
        };

        // Act
        var result = ToonSerializer.Serialize(user, options);

        // Assert
        Assert.Contains("Name", result);
        Assert.Contains("null", result);
    }
}