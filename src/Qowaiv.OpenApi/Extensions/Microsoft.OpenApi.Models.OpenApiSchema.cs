using Qowaiv.OpenApi;

namespace Microsoft.OpenApi.Models;

public static class QowaivOpenApiSchemaExtensions
{
    [Pure]
    public static OpenApiType OpenApiType(this OpenApiSchema schema)
        => Enum.TryParse<OpenApiType>(schema?.Type, out var type)
        ? type
        : default;

    [Pure]
    public static IEnumerable<OpenApiProperty> OpenApiProperties(this OpenApiSchema schema)
        => schema?.Properties.Select(kvp => new OpenApiProperty(kvp.Key, kvp.Value))
        ?? Array.Empty<OpenApiProperty>();
}
