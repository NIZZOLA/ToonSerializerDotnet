using Nizzola.ToonConvert;
using Shouldly;

namespace NizzolaToon.Tests;

public class ToonIgnoreAttributeTests
{
    [Fact]
    public void Serialize_WithToonIgnore_IgnoresProperty()
    {
        // Arrange
        var user = new TestUserWithIgnore
        {
            Id = 1,
            Name = "Alice",
            Secret = "hidden",
            InternalId = 999
        };

        // Act
        var result = ToonSerializer.Serialize(user);

        // Assert
        result.ShouldContain("Id");
        result.ShouldContain("Name");
        result.ShouldNotContain("Secret");
        result.ShouldNotContain("InternalId");
        result.ShouldNotContain("hidden");
        result.ShouldNotContain("999");
    }

    [Fact]
    public void Serialize_ListWithToonIgnore_IgnoresPropertyInAllItems()
    {
        // Arrange
        var users = new List<TestUserWithIgnore>
        {
            new() { Id = 1, Name = "Alice", Secret = "secret1" },
            new() { Id = 2, Name = "Bob", Secret = "secret2" }
        };

        // Act
        var result = ToonSerializer.Serialize(users);

        // Assert
        result.ShouldContain("{Id,Name}:");
        result.ShouldNotContain("Secret");
        result.ShouldNotContain("secret1");
        result.ShouldNotContain("secret2");
    }
}