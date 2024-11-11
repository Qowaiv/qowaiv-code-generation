using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    /// <summary>Resolves the <see cref="Type"/> based on the <see cref="ResolveOpenApiSchema.DataType"/>.</summary>
    [Pure]
    protected Type? ResolveDataType(ResolveOpenApiSchema schema) => schema.DataType switch
    {
        _ when schema.Enum.Any() /*.*/ => ResolveEnum(schema),
        OpenApiDataType.boolean /*..*/ => ResolveBoolean(schema),
        OpenApiDataType.integer /*..*/ => ResolveInteger(schema),
        OpenApiDataType.number /*...*/ => ResolveNumber(schema),
        OpenApiDataType.@string /*..*/ => ResolveString(schema),
        OpenApiDataType.array /*....*/ => ResolveArray(schema),
        OpenApiDataType.@object /*..*/ => ResolveObject(schema),
        _ => ResolveOther(schema),
    };

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.@object"/>.</summary>
    [Pure]
    protected Class ResolveObject(ResolveOpenApiSchema schema)
    {
        var data = GetTypeData(schema);

        Class @class = Settings.ModelType == ModelType.Record
            ? new Record(data)
            : new Class(data);

        schema.Context.Add(schema, @class, data);

        schema = schema with { Model = @class };

        var props = schema.Properties.Select(p => ResolveProperty(p, schema.Required)).OfType<Property>().ToArray();

        data.AddProperties(props);

        data.AddAllOfs(ResolveAllOf(schema));
        data.AddOneOfs(ResolveOneOf(schema));
        return @class;
    }

    /// <summary>
    /// Resolves the <see cref="Type"/> when there is at least one <see cref="ResolveOpenApiSchema.AllOf"/>'s.
    /// </summary>
    [Pure]
    private Type[] ResolveAllOf(ResolveOpenApiSchema schema)
    {
         return [.. schema.AllOf.Select(Resolve).OfType<Type>()];
    }

    /// <summary>
    /// Resolves the <see cref="Type"/> when there are multiple <see cref="ResolveOpenApiSchema.OneOf"/>'s.
    /// </summary>
    [Pure]
    private Type[] ResolveOneOf(ResolveOpenApiSchema schema)
    {
        return [.. schema.OneOf.Select(Resolve).OfType<Type>()];
    }

    [Pure]
    private OpenApiTypeData GetTypeData(ResolveOpenApiSchema schema)
    {
        var typeName = ResolveName(schema, ResolveTypeName.Object)!;

        var data = new OpenApiTypeData(schema.Context)
        {
            Schema = schema,
            TypeName = typeName,
            IsSealed = Settings.Sealed,
            IsPartial = Settings.Partial,
            Visibility = ResolveVisibility(schema),
            Documentation = new XmlDocumentation() { Summary = schema.Description },
        };

        return data;
    }
}
