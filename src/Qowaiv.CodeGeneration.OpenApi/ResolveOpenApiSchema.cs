using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Qowaiv.CodeGeneration.OpenApi;

public readonly partial struct ResolveOpenApiSchema(
    OpenApiPath path,
    OpenApiSchema schema,
    Type? model,
    Type? @base = null)
{
    public readonly OpenApiPath Path = path;
    public readonly OpenApiSchema Schema = schema;
    public readonly Type? Model = model;
    public readonly Type? Base = @base;

    /// <inheritdoc cref="OpenApiReference.Id" />
    public string? ReferenceId => Reference?.Id;

    /// <inheritdoc cref="OpenApiSchema.AllOf"/>
    public IEnumerable<ResolveOpenApiSchema> AllOf
    {
        get
        {
            var self = this;
            return Schema?.AllOf?.Select(self.With)
            ?? [];
        }
    }

    /// <inheritdoc cref="OpenApiSchema.AnyOf"/>
    public IEnumerable<ResolveOpenApiSchema> AnyOf
    {
        get
        {
            var self = this;
            return Schema?.AnyOf?.Select(self.With)
            ?? [];
        }
    }

    /// <inheritdoc cref="OpenApiSchema.OneOf"/>
    public IEnumerable<ResolveOpenApiSchema> OneOf
    {
        get
        {
            var self = this;
            return Schema?.OneOf?.Select(self.With)
            ?? [];
        }
    }

    /// <inheritdoc cref="OpenApiSchema.Description"/>
    public string? Description => Schema?.Description;

    /// <inheritdoc cref="OpenApiSchema.Enum"/>
    public IReadOnlyList<IOpenApiAny> Enum => Schema?.Enum.AsReadOnlyList() ?? [];

    /// <inheritdoc cref="OpenApiSchema.Required"/>
    public IReadOnlySet<string> Required => Schema?.Required.AsReadOnlySet() ?? new HashSet<string>();

    /// <inheritdoc cref="OpenApiSchema.Format"/>
    public string? Format => Schema?.Format;

    /// <inheritdoc cref="OpenApiSchema.Minimum"/>
    public decimal? Minimum => Schema?.Minimum;

    /// <inheritdoc cref="OpenApiSchema.Maximum"/>
    public decimal? Maximum => Schema?.Maximum;

    /// <inheritdoc cref="OpenApiSchema.MinLength"/>
    public int? MinLength => Schema?.MinLength;

    /// <inheritdoc cref="OpenApiSchema.MaxLength"/>
    public int? MaxLength => Schema?.MaxLength;

    /// <inheritdoc cref="OpenApiSchema.Nullable"/>
    public bool Nullable => Schema?.Nullable ?? false;

    /// <summary>The Open API type.</summary>
    public OpenApiType OpenApiType => Schema?.OpenApiType() ?? OpenApiType.None;

    /// <inheritdoc cref="OpenApiSchema.Reference"/>
    public OpenApiReference? Reference => Schema?.Reference;

    /// <inheritdoc cref="OpenApiSchema.Type"/>
    public string? Type => Schema?.Type;

    /// <summary>Gets the Properties.</summary>
    [Pure]
    public IEnumerable<ResolveOpenApiSchema> Properties
    {
        get
        {
            var self = this;
            return Schema?.Properties.Select(p => new ResolveOpenApiSchema(self.Path.Child(p.Key), p.Value, self.Model))
                ?? [];
        }
    }

    /// <inheritdoc cref="OpenApiSchema.Items"/>
    public ResolveOpenApiSchema Items => new(Path, Schema?.Items!, Model, Base);

    [Pure]
    public ResolveOpenApiSchema With(OpenApiSchema schema) => new(Path, schema, Model, Base);

    [Pure]
    public ResolveOpenApiSchema WithModel(Type model) => new(Path, Schema, model, Base);

    [Pure]
    public ResolveOpenApiSchema WithBase(Type @base) => new(Path, Schema, Model, @base);

    /// <inheritdoc />
    [Pure]
    public override string ToString()
    {
        if (Path.ToString() == string.Empty)
        {
            return "{empty}";
        }
        else
        {
            var sb = new StringBuilder($"{Path}, ");
            sb.Append("Schema = { ");

            if (!string.IsNullOrEmpty(Type))
            {
                sb.Append($"Type = {Type}, ");
            }
            if (!string.IsNullOrEmpty(Format))
            {
                sb.Append($"Format = {Format}, ");
            }

            sb.Append('}');
            sb.Replace(", }", " }");

            if (Model is { })
            {
                sb.Append($", Model = {Model.ToCSharpString()}");
            }
            return sb.ToString();
        }
    }
}
