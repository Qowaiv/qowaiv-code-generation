using Qowaiv.CodeGeneration.OpenApi;

namespace Microsoft.OpenApi.Models;

/// <summary>Extensions on <see cref="OpenApiSchema"/>.</summary>
public static class QowaivOpenApiSchemaExtensions
{
    /// <summary>
    /// Gets the <see cref="Qowaiv.CodeGeneration.OpenApi.OpenApiDataType"/> of the schema.
    /// </summary>
    [Pure]
    public static OpenApiDataType OpenApiType(this OpenApiSchema? schema)
    {
        if (schema?.Type is { } tp && Enum.TryParse<OpenApiDataType>(tp, out var type))
        {
            return type;
        }
        else if (schema?.Properties.Any() == true)
        {
            return Qowaiv.CodeGeneration.OpenApi.OpenApiDataType.@object;
        }
        else return default;
    }
}
