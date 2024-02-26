using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    /// <summary>Resolves the preferred property access.</summary>
    [Pure]
    protected virtual PropertyAccess ResolveAccess(ResolveOpenApiSchema property) => Settings.PropertyAccess;

    /// <summary>Resolves the preferred property access.</summary>
    [Pure]
    protected virtual CodeVisibility ResolveVisibility(ResolveOpenApiSchema property) => CodeVisibility.Public;

    [Pure]
    protected virtual TypeName? ResolveName(ResolveOpenApiSchema schema, ResolveTypeName type)
    {
        var name = schema.ReferenceId ?? schema.Path.Last;
        var lastDot = name.LastIndexOf('.');
        var ns = lastDot == -1 ? DefaultNamespace : DefaultNamespace.Child(name[..lastDot]);
        name = NamingStrategy.PascalCase((lastDot == -1 ? name : name[(lastDot + 1)..]).TrimStart('_'));
        return new(ns, name);
    }

    [Pure]
    private Property? ResolveProperty(ResolveOpenApiSchema schema, ISet<string> required)
    {
        if (schema.Model is Class @class && Resolve(schema) is { } propertyType)
        {
            // nullability is handled on type for structs, and on the property for classes.
            var nullable = propertyType.IsClass && !propertyType.IsArray;

            if (schema.Nullable || Settings.NullableValueTypes)
            {
                propertyType = AsNullable(propertyType);
            }

            var attributes = new List<AttributeInfo>();
            var documentation = new XmlDocumentation
            {
                Summary = schema.Description,
            };
            var prop = new Property(
                PropertyName(schema),
                propertyType,
                @class,
                ResolveAccess(schema),
                attributes,
                documentation,
                nullable,
                required.Contains(schema.Path.Last));

            attributes.AddRange(DecorateProperty(prop, schema));
            return prop;
        }
        else return null;
    }

    private Type AsNullable(Type type)
        => !type.IsValueType || type.IsNullableValueType() || IsNullableByDesign(type)
        ? type
        : typeof(Nullable<>).MakeGenericType(type);
}
