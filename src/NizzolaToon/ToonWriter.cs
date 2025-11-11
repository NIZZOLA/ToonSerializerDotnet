using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Nizzola.ToonConvert;

/// <summary>
/// Writer for TOON format with indentation support.
/// </summary>
public sealed class ToonWriter : IDisposable
{
    private readonly StringBuilder _builder;
    private readonly ToonSerializerOptions _options;

    public ToonWriter(ToonSerializerOptions options)
    {
        _builder = new StringBuilder();
        _options = options;
    }

    public void WriteValue(object? value, Type type, int indentLevel)
    {
        if (value is null)
        {
            WriteRaw("null");
            return;
        }

        // Handle primitives
        if (IsPrimitive(type))
        {
            WriteRaw(FormatPrimitive(value));
            return;
        }

        // Handle collections
        if (TryWriteCollection(value, type, indentLevel))
            return;

        // Handle objects
        WriteObject(value, type, indentLevel);
    }

    private bool IsPrimitive(Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(DateTimeOffset) ||
               type == typeof(Guid) ||
               type.IsEnum;
    }

    private string FormatPrimitive(object value)
    {
        return value switch
        {
            bool b => b ? "true" : "false",
            string s => s,
            DateTime dt => dt.ToString("o"),
            DateTimeOffset dto => dto.ToString("o"),
            Enum e => e.ToString(),
            _ => value.ToString() ?? string.Empty
        };
    }

    private bool TryWriteCollection(object value, Type type, int indentLevel)
    {
        if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(type) || type == typeof(string))
            return false;

        var enumerable = (System.Collections.IEnumerable)value;
        var items = enumerable.Cast<object>().ToList();

        if (items.Count == 0)
        {
            WriteRaw("[]");
            return true;
        }

        var itemType = type.IsGenericType
            ? type.GetGenericArguments()[0]
            : items[0]?.GetType() ?? typeof(object);

        // Check if it's tabular data (list of objects)
        if (!IsPrimitive(itemType) && items.Count > 0)
        {
            WriteTabular(items, itemType, indentLevel);
        }
        else
        {
            // Write as simple array
            WriteRaw("[");
            for (int i = 0; i < items.Count; i++)
            {
                if (i > 0) WriteRaw(",");
                WriteRaw(FormatPrimitive(items[i]));
            }
            WriteRaw("]");
        }

        return true;
    }

    private void WriteTabular(List<object> items, Type itemType, int indentLevel)
    {
        var properties = GetSerializableProperties(itemType);

        if (properties.Length == 0)
            return;

        // Get property names with naming policy applied
        var propertyNames = properties
            .Select(p => _options.PropertyNamingPolicy?.ConvertName(p.Name) ?? p.Name)
            .ToArray();

        // Write: CollectionName[count]{prop1,prop2,...}:
        var typeName = GetCollectionName(itemType);
        WriteRaw($"{typeName}[{items.Count}]{{{string.Join(",", propertyNames)}}}:");
        WriteLine();

        // Write rows with indentation
        foreach (var item in items)
        {
            WriteIndent(indentLevel + 1);
            var values = properties.Select(p => FormatPropertyValue(p.GetValue(item), p.PropertyType, indentLevel + 1));
            WriteRaw(string.Join(",", values));
            WriteLine();
        }
    }

    private string GetCollectionName(Type itemType)
    {
        var name = itemType.Name;

        // Apply naming policy if exists
        if (_options.PropertyNamingPolicy != null)
        {
            name = _options.PropertyNamingPolicy.ConvertName(name);
        }

        // Make plural (simple approach)
        if (!name.EndsWith("s", StringComparison.OrdinalIgnoreCase))
        {
            name = name + "s";
        }

        return name.ToLowerInvariant();
    }

    private void WriteObject(object value, Type type, int indentLevel)
    {
        var properties = GetSerializableProperties(type);

        if (properties.Length == 0)
            return;

        // Get property names
        var propertyNames = properties
            .Select(p => _options.PropertyNamingPolicy?.ConvertName(p.Name) ?? p.Name)
            .ToArray();

        // Write: ObjectName{prop1,prop2,...}:
        var typeName = _options.PropertyNamingPolicy?.ConvertName(type.Name) ?? type.Name;
        WriteRaw($"{typeName.ToLowerInvariant()}{{{string.Join(",", propertyNames)}}}:");
        WriteLine();

        // Write values - check each property for nested collections/objects
        foreach (var prop in properties)
        {
            var propValue = prop.GetValue(value);

            if (propValue is null && _options.IgnoreNullValues)
                continue;

            var propName = _options.PropertyNamingPolicy?.ConvertName(prop.Name) ?? prop.Name;

            // Check if property is a collection of complex objects
            if (IsComplexCollection(prop.PropertyType, propValue))
            {
                WriteIndent(indentLevel + 1);
                WriteRaw($"{propName}:");
                WriteLine();
                WriteValue(propValue, prop.PropertyType, indentLevel + 1);
            }
            else
            {
                // Write simple property inline
                WriteIndent(indentLevel + 1);
                WriteRaw($"{propName}:{FormatPropertyValue(propValue, prop.PropertyType, indentLevel + 1)}");
                WriteLine();
            }
        }
    }

    private bool IsComplexCollection(Type type, object? value)
    {
        if (value is null)
            return false;

        if (!typeof(System.Collections.IEnumerable).IsAssignableFrom(type) || type == typeof(string))
            return false;

        var itemType = type.IsGenericType
            ? type.GetGenericArguments()[0]
            : typeof(object);

        return !IsPrimitive(itemType);
    }

    private PropertyInfo[] GetSerializableProperties(Type type)
    {
        return type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetCustomAttribute<ToonIgnoreAttribute>() == null)
            .OrderBy(p => p.Name) // Ordenar para consistência
            .ToArray();
    }

    private string FormatPropertyValue(object? value, Type propertyType, int indentLevel)
    {
        if (value is null)
            return "null";

        // Check if it's a collection
        if (typeof(System.Collections.IEnumerable).IsAssignableFrom(propertyType) && propertyType != typeof(string))
        {
            var enumerable = (System.Collections.IEnumerable)value;
            var items = enumerable.Cast<object>().ToList();

            if (items.Count == 0)
                return "[]";

            var itemType = propertyType.IsGenericType
                ? propertyType.GetGenericArguments()[0]
                : items[0]?.GetType() ?? typeof(object);

            // If it's a collection of complex objects, it should be handled by WriteObject
            // This should only format simple arrays
            if (IsPrimitive(itemType))
            {
                var formattedItems = items.Select(i => EscapeValue(FormatPrimitive(i))).ToList();
                return $"[{string.Join(",", formattedItems)}]";
            }
            else
            {
                // Complex collection - should not reach here if WriteObject handles it correctly
                // But if it does, return a placeholder
                return $"[{items.Count} items]";
            }
        }

        return EscapeValue(FormatPrimitive(value));
    }

    private string EscapeValue(string value)
    {
        // Escape special characters if needed
        if (value.Contains(',') || value.Contains(':') || value.Contains('{') ||
            value.Contains('}') || value.Contains('[') || value.Contains(']') ||
            value.Contains(' '))
        {
            return $"\"{value}\"";
        }

        return value;
    }

    public void WritePropertyName(string name, int indentLevel)
    {
        WriteIndent(indentLevel);
        WriteRaw(name);
        WriteLine();
    }

    public void WriteIndent(int level)
    {
        _builder.Append(new string(' ', level * _options.IndentSize));
    }

    public void WriteRaw(string value)
    {
        _builder.Append(value);
    }

    public void WriteLine()
    {
        _builder.AppendLine();
    }

    public override string ToString() => _builder.ToString();

    public void Dispose()
    {
        _builder.Clear();
    }
}