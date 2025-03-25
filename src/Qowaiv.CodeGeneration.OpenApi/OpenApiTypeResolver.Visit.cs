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
        var path = OpenApiPath.Root.Child("components").Child("responses");

        foreach (var components in documents.Select(d => d.Components))
        {
            Visit(components, context);

            if (components.Responses is { } responses)
            {
                Visit(responses, path, context);
            }
        }
        foreach (var paths in documents.Select(p => p.Paths))
        {
            Visit(paths, context);
        }

        return context.ResolvedCode(DecorateModel);
    }

    private void Visit(OpenApiComponents components, OpenApiResolveContext context)
    {
        var path = OpenApiPath.Root.Child("components").Child("schemas");

        foreach ((var name, var schema) in components.Schemas)
        {
            Visit(new ResolveOpenApiSchema(path.Child(name), schema, context, null));
        }
    }

    private void Visit(OpenApiPaths paths, OpenApiResolveContext context)
    {
        var root = OpenApiPath.Root.Child("paths");

        foreach ((var pathName, var pathItem) in paths)
        {
            if (pathItem is null) continue;

            foreach ((var operationType, var operation) in pathItem.Operations)
            {
                if (operation is null) continue;

                var name = pathName.Replace('/', '_').Trim('_');

                var path = root.Child(operationType.ToString().ToLowerInvariant());

                Visit(operation.RequestBody, path.Child(name + "Request"), context);
                Visit(operation.Responses, path.Child(name + "Response"), context);
            }
        }
    }

    private void Visit(OpenApiRequestBody requestBody, OpenApiPath path, OpenApiResolveContext context)
    {
        if (requestBody?.Content is null) return;

        Visit(requestBody.Content, path, context);
    }

    private void Visit(IDictionary<string, OpenApiResponse> responses, OpenApiPath path, OpenApiResolveContext context)
    {
        foreach ((var responseType, var response) in responses)
        {
            Visit(response.Content, path.Parent.Child(responseType).Child(path.Last), context);
        }
    }

    private void Visit(IDictionary<string, OpenApiMediaType> content, OpenApiPath path, OpenApiResolveContext context)
    {
        foreach (var media in content.Values.OfType<OpenApiMediaType>().Where(m => m.Schema is { }))
        {
            var childPath = path.Parent.Child(path.Last);
            var resolve = new ResolveOpenApiSchema(childPath, media.Schema, context, null);
            Visit(resolve);
        }
    }

    private void Visit(ResolveOpenApiSchema schema) => schema.Context.Add(schema, Resolve(schema));
}
