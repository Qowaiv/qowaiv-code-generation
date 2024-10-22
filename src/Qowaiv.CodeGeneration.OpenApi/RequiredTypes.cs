namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>
/// How to determine if a property is required or not.
/// </summary>
[Flags]
public enum RequiredTypes
{
    /// <summary>No property will be required.</summary>
    None = 0,

    /// <summary>Properties are required if not nullable.</summary>
    Nullabillity = /*.*/ 0b001,

    /// <summary>Properties are required if defined as such.</summary>
    IsRequired = /*...*/ 0b010,

    /// <summary>All properties are required.</summary>
    All = /*..........*/ 0b100,
}
