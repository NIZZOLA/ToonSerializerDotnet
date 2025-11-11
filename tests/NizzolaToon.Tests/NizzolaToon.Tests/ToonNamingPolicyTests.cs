using Nizzola.ToonConvert;

namespace NizzolaToon.Tests;

public class ToonNamingPolicyTests
{
    [Theory]
    [InlineData("FirstName", "firstName")]
    [InlineData("ID", "iD")]
    [InlineData("name", "name")]
    [InlineData("A", "a")]
    [InlineData("", "")]
    public void CamelCase_ConvertName_ReturnsCorrectResult(string input, string expected)
    {
        // Arrange
        var policy = ToonNamingPolicy.CamelCase;

        // Act
        var result = policy.ConvertName(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Theory]
    [InlineData("FirstName", "first_name")]
    [InlineData("UserID", "user_i_d")]
    [InlineData("name", "name")]
    [InlineData("HTTPRequest", "h_t_t_p_request")]
    [InlineData("A", "a")]
    [InlineData("", "")]
    public void SnakeCase_ConvertName_ReturnsCorrectResult(string input, string expected)
    {
        // Arrange
        var policy = ToonNamingPolicy.SnakeCase;

        // Act
        var result = policy.ConvertName(input);

        // Assert
        Assert.Equal(expected, result);
    }

    [Fact]
    public void CamelCase_WithNull_ReturnsNull()
    {
        // Arrange
        var policy = ToonNamingPolicy.CamelCase;

        // Act
        var result = policy.ConvertName(null!);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SnakeCase_WithNull_ReturnsNull()
    {
        // Arrange
        var policy = ToonNamingPolicy.SnakeCase;

        // Act
        var result = policy.ConvertName(null!);

        // Assert
        Assert.Null(result);
    }
}