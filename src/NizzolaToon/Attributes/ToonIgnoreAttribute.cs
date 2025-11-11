using System;

namespace Nizzola.ToonConvert;

/// <summary>
/// Indicates that a property should be ignored during TOON serialization.
/// </summary>
[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
public sealed class ToonIgnoreAttribute : Attribute
{
}
