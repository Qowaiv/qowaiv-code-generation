using Microsoft.OpenApi.Any;
using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    public OpenApiTypeResolver(Namespace defaultNamespace, OpenApiTypeResolverSettings? settings = null)
    {
        DefaultNamespace = Guard.NotDefault(defaultNamespace);
        Settings = settings ?? new OpenApiTypeResolverSettings();
    }

    public Namespace DefaultNamespace { get; }
    private readonly OpenApiTypeResolverSettings Settings;

    /// <remarks>
    /// Types that are null-able by design will not be transformed to 
    /// <see cref="Nullable{T}"/>'s when optional.
    /// </remarks>
    [Pure]
    protected virtual bool IsNullableByDesign(Type type)
        => type is { IsValueType: true }
        && (type.IsNullableValueType() || HasNoneOrEmptyField(type));

    private static bool HasNoneOrEmptyField(Type type)
        => type.GetFields(BindingFlags.Static | BindingFlags.Public)
            .Any(f => f.FieldType == type && (f.Name == "Empty" || f.Name == "None"));

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
    protected virtual string PropertyName(ResolveOpenApiSchema schema)
        => NamingStrategy.PascalCase(schema.Path.Last, schema.Model!);

    [Pure]
    protected virtual string EnumValueName(Enumeration @enum, OpenApiString enumValue)
        => NamingStrategy.Enum(Guard.NotNull(enumValue).Value);

    [Pure]
    protected virtual string NormalizeFormat(string? str)
        => (str ?? string.Empty).ToUpperInvariant().Replace("-", "");
}
