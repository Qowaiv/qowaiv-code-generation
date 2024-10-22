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
        => resolved.TryAdd(schema.ReferenceId!, Resolve(schema));
}
