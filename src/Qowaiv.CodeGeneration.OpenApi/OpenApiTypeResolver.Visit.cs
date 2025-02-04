using Microsoft.OpenApi.Models;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    /// <summary>Walk through the <see cref="OpenApiDocument"/>.</summary>
    [Pure]
    public IReadOnlyCollection<Code> Walk(OpenApiDocument document) => Walk([document]);

    /// <summary>Walk through the <see cref="OpenApiDocument"/>.</summary>
    [Pure]
    public IReadOnlyCollection<Code> Walk(IEnumerable<OpenApiDocument> documents)
    {
        Guard.NotNull(documents);

        var context = new OpenApiResolveContext();

        foreach (var components in documents.Select(d => d.Components))
        {
            Visit(components, context);

            if (components.Responses is { } responses)
            {
                Visit(responses, context);
            }
        }

        return context.ResolvedCode(DecorateModel);
    }

    private void Visit(OpenApiComponents components, OpenApiResolveContext context)
    {
        var path = OpenApiPath.Root.Child("components").Child("schemas");

        foreach (var kvp in components.Schemas)
        {
            Visit(new ResolveOpenApiSchema(path.Child(kvp.Key), kvp.Value, context, null));
        }
    }

    private void Visit(IDictionary<string, OpenApiResponse> responses, OpenApiResolveContext context)
    {
        var path = OpenApiPath.Root.Child("components").Child("responses");

        foreach (var response in responses.Where(kvp => kvp.Key is { Length: > 0 }))
        {
            foreach (var schema in response.Value.Content.Values.Select(v => v.Schema))
            {
                var resolve = new ResolveOpenApiSchema(path.Child(response.Key), schema, context, null);
                Visit(resolve);
            }
        }
    }

    private void Visit(ResolveOpenApiSchema schema) => schema.Context.Add(schema, Resolve(schema));
}
