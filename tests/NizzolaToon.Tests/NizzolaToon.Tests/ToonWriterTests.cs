using Nizzola.ToonConvert;

namespace NizzolaToon.Tests;

public class ToonWriterTests
{
    [Fact]
    public void WriteValue_Null_WritesNull()
    {
        // Arrange
        var options = ToonSerializerOptions.Default;
        using var writer = new ToonWriter(options);

        // Act
        writer.WriteValue(null, typeof(object), 0);
        var result = writer.ToString();

        // Assert
        Assert.Equal("null", result);
    }

    [Fact]
    public void WriteIndent_CreatesCorrectSpacing()
    {
        // Arrange
        var options = new ToonSerializerOptions { IndentSize = 2 };
        using var writer = new ToonWriter(options);

        // Act
        writer.WriteIndent(3);
        var result = writer.ToString();

        // Assert
        Assert.Equal("      ", result); // 6 espaços (3 * 2)
    }

    [Fact]
    public void WriteRaw_AppendsText()
    {
        // Arrange
        var options = ToonSerializerOptions.Default;
        using var writer = new ToonWriter(options);

        // Act
        writer.WriteRaw("test");
        writer.WriteRaw("123");
        var result = writer.ToString();

        // Assert
        Assert.Equal("test123", result);
    }

    [Fact]
    public void WriteLine_AddsNewLine()
    {
        // Arrange
        var options = ToonSerializerOptions.Default;
        using var writer = new ToonWriter(options);

        // Act
        writer.WriteRaw("line1");
        writer.WriteLine();
        writer.WriteRaw("line2");
        var result = writer.ToString();

        // Assert
        Assert.True(result.Contains("line1") && result.Contains("line2"));
    }

    [Fact]
    public void Dispose_ClearsBuilder()
    {
        // Arrange
        var options = ToonSerializerOptions.Default;
        var writer = new ToonWriter(options);
        writer.WriteRaw("test");

        // Act
        writer.Dispose();
        var result = writer.ToString();

        // Assert
        Assert.Empty(result);
    }
}