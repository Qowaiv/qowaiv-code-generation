using Qowaiv.OpenApi;

namespace Microsoft.OpenApi.Models;

public static class QowaivOpenApiSchemaExtensions
{
    public static IEnumerable<OpenApiNamedSchema> NamedSchemas(this OpenApiComponents components)
        => components.Schemas.Select(kvp => new OpenApiNamedSchema(kvp.Key, kvp.Value));
}
