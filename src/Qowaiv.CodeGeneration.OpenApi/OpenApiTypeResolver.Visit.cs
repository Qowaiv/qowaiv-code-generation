using Microsoft.OpenApi.Models;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    private readonly Dictionary<string, Type?> resolved = [];
    private readonly Dictionary<TypeName, TypeInfo> infos = [];

    [Pure]
    public IReadOnlyCollection<Code> Walk(OpenApiDocument document)
    {
        Guard.NotNull(document);

        if (document.Components is { } components)
        {
            var path = OpenApiPath.Root.Child("components").Child("schemas");

            foreach (var kvp in components.Schemas)
            {
                Visit(new ResolveOpenApiSchema(path.Child(kvp.Key), kvp.Value, null));
            }

            if (components.Responses is { } responses)
            {
                path = OpenApiPath.Root.Child("components").Child("responses");
                foreach (var response in responses.Where(kvp => kvp.Key is { Length: > 0 }))
                {
                    foreach (var schema in response.Value.Content.Values.Select(v => v.Schema))
                    {
                        var resolve = new ResolveOpenApiSchema(path.Child(response.Key), schema, null);
                        Visit(resolve);
                    }
                }
            }
        }
        return ResolvedCode();
    }

    [Pure]
    public IReadOnlyCollection<Code> ResolvedCode()
    {
        var collection = resolved.Values.OfType<Code>().ToArray();
        resolved.Clear();
        return collection;
    }

    private void Visit(ResolveOpenApiSchema schema)
    {
        var id = schema.ReferenceId ?? schema.Path.ToString();
        resolved.TryAdd(id, Resolve(schema));
    }
}
