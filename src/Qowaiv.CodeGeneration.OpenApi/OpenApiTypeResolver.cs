using Microsoft.OpenApi.Any;
using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Diagnostics.Contracts;
using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>
/// Resolver that collects/resolves <see cref="Type"/> from an
/// <see cref="Microsoft.OpenApi.Models.OpenApiSchema"/>.
/// </summary>
/// <param name="defaultNamespace">
/// The default namespace for the resolved types.
/// </param>
/// <param name="settings">
/// The (optional) settings to apply.
/// </param>
public partial class OpenApiTypeResolver(
    Namespace defaultNamespace,
    OpenApiTypeResolverSettings? settings = null)
{
    /// <summary>Indicates that the resolved type should be ignored.</summary>
    public readonly Type Ignore = typeof(IgnoreType);

    /// <summary>The default namespace for the resolved code.</summary>
    public Namespace DefaultNamespace { get; } = Guard.NotDefault(defaultNamespace);

    /// <summary>The resolver settings to apply.</summary>
    private readonly OpenApiTypeResolverSettings Settings = settings ?? new OpenApiTypeResolverSettings();

    /// <summary>
    /// Returns true for <see cref="Nullable{T}"/> and value types with a None
    /// or Empty field.
    /// </summary>
    /// <remarks>
    /// Types that are null-able by design will not be transformed to
    /// <see cref="Nullable{T}"/>'s when optional.
    /// </remarks>
    [Pure]
    protected virtual bool IsNullableByDesign(Type type)
        => type is { IsValueType: true }
        && (type.IsNullableValueType() || HasNoneOrEmptyField(type));

    [Pure]
    private static bool HasNoneOrEmptyField(Type type)
        => Array.Exists(
            array: type.GetFields(BindingFlags.Static | BindingFlags.Public),
            match: f => f.FieldType == type && (f.Name == "Empty" || f.Name == "None"))
        || Array.Exists(
            array: type.GetProperties(BindingFlags.Static | BindingFlags.Public),
            match: p => p.PropertyType == type && (p.Name == "Empty" || p.Name == "None"));

    [Pure]
    protected virtual EnumerationField ResolveEnumField(OpenApiString @enum, Enumeration type)
    {
        Guard.NotNull(@enum);

        object? value = int.TryParse(@enum.Value, out var number)
            ? number
            : null;

        var attributes = new List<AttributeInfo>();
        var field = new EnumerationField(type, EnumValueName(type, @enum), value, attributes);
        attributes.AddRange(DecorateEnumerationField(field, @enum));

        return field;
    }

    [Pure]
    protected virtual CodeName PropertyName(ResolveOpenApiSchema schema)
        => CodeName.Create(schema.Path.Last, CodeNameConvention.PascalCase);

    [Pure]
    protected virtual string EnumValueName(Enumeration @enum, OpenApiString enumValue) => CodeName
            .Create(Guard.NotNull(enumValue).Value, OpenApiEnumNameConvention.Instance)
            .ToString();

    [Pure]
    protected virtual string NormalizeFormat(string? str)
        => (str ?? string.Empty).ToUpperInvariant().Replace("-", string.Empty);

    [EmptyClass("Helper to indicate that something should not be ignored")]
    private sealed class IgnoreType { }
}
