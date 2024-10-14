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
        var codeName = CodeName.Create(lastDot == -1 ? name : name[(lastDot + 1)..], CodeNameConvention.PascalCase);
        return new(ns, codeName.ToString());
    }

    [Pure]
    private Property? ResolveProperty(ResolveOpenApiSchema schema, IReadOnlySet<string> required)
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

            var isRequired = IsRequired(schema, propertyType);

            nullable &= Settings.NullableRequiredTypes || !isRequired;

            var prop = new Property(
                PropertyName(schema),
                propertyType,
                @class,
                ResolveAccess(schema),
                attributes,
                documentation,
                nullable,
                isRequired);

            attributes.AddRange(DecorateProperty(prop, schema));
            return prop;
        }
        else return null;

        bool IsRequired(ResolveOpenApiSchema schema, Type propertyType)
        {
            var isRequired = Settings.RequiredType.HasFlag(RequiredTypes.All);
            isRequired |= Settings.RequiredType.HasFlag(RequiredTypes.IsRequired) && required.Contains(schema.Path.Last);
            isRequired |= Settings.RequiredType.HasFlag(RequiredTypes.Nullabillity) && !schema.Nullable && !propertyType.IsValueType;
            return isRequired;
        }
    }

    [Pure]
    private Type AsNullable(Type type)
        => !type.IsValueType || type.IsNullableValueType() || IsNullableByDesign(type)
        ? type
        : typeof(Nullable<>).MakeGenericType(type);
}
