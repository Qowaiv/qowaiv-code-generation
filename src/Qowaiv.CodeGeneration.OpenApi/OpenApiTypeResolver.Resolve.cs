using Qowaiv.CodeGeneration.OpenApi.Collections;
using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    /// <summary>Resolve the <see cref="Type"/> defined by the (sub) schema.</summary>
    [Pure]
    protected Type? Resolve(ResolveOpenApiSchema schema)
    {
        var context = Guard.NotNull(schema).Context;

        return context.Get(schema)
            ?? context.Add(schema, ResolveCustomization(schema) ?? ResolveDataType(schema));
    }

    /// <summary>Custom resolving.</summary>
    [Pure]
    protected virtual Type? ResolveCustomization(ResolveOpenApiSchema schema) => null;

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.None"/>.</summary>
    [Pure]
    protected virtual Type? ResolveOther(ResolveOpenApiSchema schema)
        => throw new NotSupportedException($"Schema '{schema.Path}' with type '{schema.Type}' is not supported.");

    /// <summary>Resolves the preferred property access.</summary>
    [Pure]
    protected virtual PropertyAccess ResolveAccess(ResolveOpenApiSchema property) => Settings.PropertyAccess;

    /// <summary>Resolves the preferred property access.</summary>
    [Pure]
    protected virtual CodeVisibility ResolveVisibility(ResolveOpenApiSchema property) => CodeVisibility.Public;

    [Pure]
    protected virtual TypeName? ResolveName(ResolveOpenApiSchema schema, ResolveTypeName type)
    {
        Guard.NotNull(schema);

        var name = schema.ReferenceId.HasValue ? schema.ReferenceId.ToString() : schema.Path.Last;
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

            var attributes = Empty.HashSet<AttributeInfo>();
            var isRequired = IsRequired(schema, propertyType);

            nullable &= Settings.NullableRequiredTypes || !isRequired;

            var data = new PropertyData
            {
                Name = PropertyName(schema),
                PropertyType = propertyType,
                DeclaringType = @class,
                PropertyAccess = ResolveAccess(schema),
                Attributes = attributes,
                Documentation = new() { Summary = schema.Description },
                IsNullable = nullable,
                IsRequired = isRequired,
            };

            var prop = new Property(data);
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
