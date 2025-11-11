using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Nizzola.ToonConvert;

/// <summary>
/// Provides functionality to serialize objects to TOON (Token-Oriented Object Notation) format.
/// </summary>
public static class ToonSerializer
{
    /// <summary>
    /// Serializes an object to TOON format string.
    /// </summary>
    public static string Serialize<T>(T value, ToonSerializerOptions? options = null)
    {
        options ??= ToonSerializerOptions.Default;

        using var writer = new ToonWriter(options);
        writer.WriteValue(value, typeof(T), 0);
        return writer.ToString();
    }

    /// <summary>
    /// Serializes an object to TOON format and writes to a Stream.
    /// </summary>
    public static void Serialize<T>(Stream stream, T value, ToonSerializerOptions? options = null)
    {
        var toonString = Serialize(value, options);
        using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        writer.Write(toonString);
    }

    /// <summary>
    /// Serializes an object to TOON format asynchronously.
    /// </summary>
    public static async Task SerializeAsync<T>(Stream stream, T value, ToonSerializerOptions? options = null, CancellationToken cancellationToken = default)
    {
        var toonString = Serialize(value, options);
        await using var writer = new StreamWriter(stream, Encoding.UTF8, leaveOpen: true);
        await writer.WriteAsync(toonString.AsMemory(), cancellationToken);
    }

    /// <summary>
    /// Converts JSON string to TOON format.
    /// </summary>
    public static string FromJson(string json, ToonSerializerOptions? options = null)
    {
        using var document = JsonDocument.Parse(json);
        return FromJsonElement(document.RootElement, options);
    }

    /// <summary>
    /// Converts JsonElement to TOON format.
    /// </summary>
    public static string FromJsonElement(JsonElement element, ToonSerializerOptions? options = null)
    {
        options ??= ToonSerializerOptions.Default;
        using var writer = new ToonWriter(options);
        WriteJsonElement(writer, element, 0, null);
        return writer.ToString();
    }

    private static void WriteJsonElement(ToonWriter writer, JsonElement element, int indentLevel, string? propertyName)
    {
        switch (element.ValueKind)
        {
            case JsonValueKind.Object:
                WriteJsonObject(writer, element, indentLevel, propertyName);
                break;
            case JsonValueKind.Array:
                WriteJsonArray(writer, element, indentLevel, propertyName);
                break;
            case JsonValueKind.String:
                writer.WriteRaw(EscapeValue(element.GetString() ?? string.Empty));
                break;
            case JsonValueKind.Number:
                writer.WriteRaw(element.GetRawText());
                break;
            case JsonValueKind.True:
                writer.WriteRaw("true");
                break;
            case JsonValueKind.False:
                writer.WriteRaw("false");
                break;
            case JsonValueKind.Null:
                writer.WriteRaw("null");
                break;
        }
    }

    private static void WriteJsonObject(ToonWriter writer, JsonElement element, int indentLevel, string? propertyName)
    {
        var properties = element.EnumerateObject().ToList();

        if (properties.Count == 0)
            return;

        var propertyNames = properties.Select(p => p.Name).ToArray();
        var objectName = propertyName ?? "object";

        // Write: name{prop1,prop2,...}:
        writer.WriteRaw($"{objectName}{{{string.Join(",", propertyNames)}}}:");
        writer.WriteLine();

        // Write values
        writer.WriteIndent(indentLevel + 1);
        var values = properties.Select(p => GetSimpleValue(p.Value));
        writer.WriteRaw(string.Join(",", values));
        writer.WriteLine();
    }

    private static void WriteJsonArray(ToonWriter writer, JsonElement element, int indentLevel, string? propertyName)
    {
        var array = element.EnumerateArray().ToList();

        if (array.Count == 0)
        {
            writer.WriteRaw("[]");
            return;
        }

        // Check if array contains objects with same structure (tabular data)
        if (array.All(e => e.ValueKind == JsonValueKind.Object) && array.Count > 0)
        {
            WriteTabularArray(writer, array, indentLevel, propertyName);
        }
        else
        {
            // Write as simple array
            writer.WriteRaw("[");
            for (int i = 0; i < array.Count; i++)
            {
                if (i > 0) writer.WriteRaw(",");
                writer.WriteRaw(GetSimpleValue(array[i]));
            }
            writer.WriteRaw("]");
        }
    }

    private static void WriteTabularArray(ToonWriter writer, List<JsonElement> array, int indentLevel, string? propertyName)
    {
        // Get all unique property names
        var allProperties = array
            .SelectMany(obj => obj.EnumerateObject().Select(p => p.Name))
            .Distinct()
            .ToList();

        if (allProperties.Count == 0)
            return;

        var arrayName = propertyName ?? "items";

        // Write: name[count]{prop1,prop2,...}:
        writer.WriteRaw($"{arrayName}[{array.Count}]{{{string.Join(",", allProperties)}}}:");
        writer.WriteLine();

        // Write rows
        foreach (var obj in array)
        {
            writer.WriteIndent(indentLevel + 1);
            var values = new List<string>();

            foreach (var propName in allProperties)
            {
                if (obj.TryGetProperty(propName, out var value))
                {
                    values.Add(GetSimpleValue(value));
                }
                else
                {
                    values.Add("null");
                }
            }

            writer.WriteRaw(string.Join(",", values));
            writer.WriteLine();
        }
    }

    private static string GetSimpleValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => EscapeValue(element.GetString() ?? string.Empty),
            JsonValueKind.Number => element.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Null => "null",
            JsonValueKind.Array => "[" + string.Join(",", element.EnumerateArray().Select(GetSimpleValue)) + "]",
            _ => element.GetRawText()
        };
    }

    private static string EscapeValue(string value)
    {
        if (string.IsNullOrEmpty(value))
            return value;

        // Escape if contains special characters
        if (value.Contains(',') || value.Contains(':') || value.Contains('{') ||
            value.Contains('}') || value.Contains('[') || value.Contains(']') ||
            value.Contains(' ') || value.Contains('\n') || value.Contains('\r'))
        {
            return $"\"{value.Replace("\"", "\\\"")}\"";
        }

        return value;
    }
}