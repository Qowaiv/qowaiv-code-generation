using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.OpenApi;

namespace Microsoft.OpenApi.Models;

/// <summary>Extensions on <see cref="OpenApiSchema"/>.</summary>
public static class QowaivOpenApiSchemaExtensions
{
    /// <summary>
    /// Gets the <see cref="Qowaiv.CodeGeneration.OpenApi.OpenApiDataType"/> of the schema.
    /// </summary>
    [Pure]
    public static OpenApiDataType DataType(this OpenApiSchema schema)
    {
        Guard.NotNull(schema);

        if (schema.Type is { } tp && Enum.TryParse<OpenApiDataType>(tp, out var type))
        {
            return type;
        }
        else if (schema.Properties.Any()
            || schema.AllOf.Any()
            || schema.OneOf.Any())
        {
            return OpenApiDataType.@object;
        }
        else return default;
    }
}
