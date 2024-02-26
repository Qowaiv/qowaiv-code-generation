using Qowaiv.CodeGeneration.OpenApi;

namespace Microsoft.OpenApi.Models;

/// <summary>Extensions on <see cref="OpenApiSchema"/>.</summary>
public static class QowaivOpenApiSchemaExtensions
{
    /// <summary>
    /// Gets the <see cref="Qowaiv.CodeGeneration.OpenApi.OpenApiType"/> of the schema.
    /// </summary>
    [Pure]
    public static OpenApiType OpenApiType(this OpenApiSchema? schema)
    {
        if (schema?.Type is { } tp && Enum.TryParse<OpenApiType>(tp, out var type))
        {
            return type;
        }
        else if (schema?.Properties.Any() == true)
        {
            return Qowaiv.CodeGeneration.OpenApi.OpenApiType.@object;
        }
        else return default;
    }

    /// <summary>Gets <see cref="OpenApiProperty"/>'s of the schema.</summary>
    [Pure]
    public static IEnumerable<OpenApiProperty> OpenApiProperties(this OpenApiSchema? schema)
        => schema?.Properties.Select(kvp => new OpenApiProperty(kvp.Key, kvp.Value))
        ?? [];
}
