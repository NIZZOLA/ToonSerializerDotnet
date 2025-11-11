namespace Nizzola.ToonConvert;

/// <summary>
/// Options for TOON serialization.
/// </summary>
public sealed class ToonSerializerOptions
{
    /// <summary>
    /// Default options instance.
    /// </summary>
    public static ToonSerializerOptions Default { get; } = new();

    /// <summary>
    /// Gets or sets the number of spaces per indentation level.
    /// </summary>
    public int IndentSize { get; set; } = 2;

    /// <summary>
    /// Gets or sets whether to ignore null values.
    /// </summary>
    public bool IgnoreNullValues { get; set; } = true;

    /// <summary>
    /// Gets or sets the naming policy for property names.
    /// </summary>
    public ToonNamingPolicy? PropertyNamingPolicy { get; set; }

    /// <summary>
    /// Gets or sets whether to flatten nested objects when possible.
    /// </summary>
    public bool FlattenNestedObjects { get; set; } = true;

    /// <summary>
    /// Gets or sets the maximum nesting depth.
    /// </summary>
    public int MaxDepth { get; set; } = 64;
}

/// <summary>
/// Base class for naming policies.
/// </summary>
public abstract class ToonNamingPolicy
{
    /// <summary>
    /// Camel case naming policy (firstName).
    /// </summary>
    public static ToonNamingPolicy CamelCase { get; } = new CamelCaseNamingPolicy();

    /// <summary>
    /// Snake case naming policy (first_name).
    /// </summary>
    public static ToonNamingPolicy SnakeCase { get; } = new SnakeCaseNamingPolicy();

    /// <summary>
    /// Converts a property name according to the policy.
    /// </summary>
    public abstract string ConvertName(string name);

    private sealed class CamelCaseNamingPolicy : ToonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name) || char.IsLower(name[0]))
                return name;
            
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
    }

    private sealed class SnakeCaseNamingPolicy : ToonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            if (string.IsNullOrEmpty(name))
                return name;

            var result = new System.Text.StringBuilder();
            result.Append(char.ToLowerInvariant(name[0]));

            for (int i = 1; i < name.Length; i++)
            {
                if (char.IsUpper(name[i]))
                {
                    result.Append('_');
                    result.Append(char.ToLowerInvariant(name[i]));
                }
                else
                {
                    result.Append(name[i]);
                }
            }

            return result.ToString();
        }
    }
}
