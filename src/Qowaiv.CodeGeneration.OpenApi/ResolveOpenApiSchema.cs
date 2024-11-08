#pragma warning disable S2365 // Properties should not make collection or array copies

using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Qowaiv.Text;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>An helper context that combines the <see cref="OpenApiSchema"/> with the models to generate.</summary>
public sealed record ResolveOpenApiSchema
{
    /// <summary>Initializes a new instance of the <see cref="ResolveOpenApiSchema"/> class.</summary>
    public ResolveOpenApiSchema(
        OpenApiPath path,
        OpenApiSchema schema,
        OpenApiResolveContext context,
        Type? model)
    {
        Path = Guard.NotDefault(path);
        Schema = Guard.NotNull(schema);
        Context = Guard.NotNull(context);
        Model = model;
    }

    /// <summary>The path of the <see cref="OpenApiSchema"/> in the full schema.</summary>
    public OpenApiPath Path { get; }

    /// <summary>The <see cref="OpenApiSchema"/> to resolve.</summary>
    public OpenApiSchema Schema { get; }

    /// <summary>The resolving context.</summary>
    public OpenApiResolveContext Context { get; }

    /// <summary>The linked model.</summary>
    public Type? Model { get; init; }

    /// <inheritdoc cref="OpenApiReference.Id" />
    public ReferenceId ReferenceId => ReferenceId.Parse(Reference?.Id);

    /// <inheritdoc cref="OpenApiSchema.AllOf"/>
    public IReadOnlyList<ResolveOpenApiSchema> AllOf => Schema.AllOf?.Select(SelectionOf).ToArray() ?? [];

    /// <inheritdoc cref="OpenApiSchema.AnyOf"/>
    public IReadOnlyList<ResolveOpenApiSchema> AnyOf => Schema.AnyOf?.Select(SelectionOf).ToArray() ?? [];

    /// <inheritdoc cref="OpenApiSchema.OneOf"/>
    public IReadOnlyList<ResolveOpenApiSchema> OneOf => Schema.OneOf?.Select(SelectionOf).ToArray() ?? [];

    /// <inheritdoc cref="OpenApiSchema.Description"/>
    public string? Description => Schema.Description;

    /// <inheritdoc cref="OpenApiSchema.Enum"/>
    public IReadOnlyList<IOpenApiAny> Enum => Schema.Enum.AsReadOnlyList() ?? [];

    /// <inheritdoc cref="OpenApiSchema.Required"/>
    public IReadOnlySet<string> Required => Schema.Required.AsReadOnlySet() ?? new HashSet<string>();

    /// <inheritdoc cref="OpenApiSchema.Format"/>
    public string? Format => Schema.Format;

    /// <inheritdoc cref="OpenApiSchema.Minimum"/>
    public decimal? Minimum => Schema.Minimum;

    /// <inheritdoc cref="OpenApiSchema.Maximum"/>
    public decimal? Maximum => Schema.Maximum;

    /// <inheritdoc cref="OpenApiSchema.MinLength"/>
    public int? MinLength => Schema.MinLength;

    /// <inheritdoc cref="OpenApiSchema.MaxLength"/>
    public int? MaxLength => Schema.MaxLength;

    /// <inheritdoc cref="OpenApiSchema.Nullable"/>
    public bool Nullable => Schema.Nullable;

    /// <summary>
    /// True if the Open API type is <see cref="OpenApiDataType.@string"/>.
    /// </summary>
    public bool IsString => DataType == OpenApiDataType.@string;

    /// <summary>The Open API data type.</summary>
    public OpenApiDataType DataType => Schema.DataType();

    /// <inheritdoc cref="OpenApiSchema.Reference"/>
    public OpenApiReference? Reference => Schema.Reference;

    /// <inheritdoc cref="OpenApiSchema.Type"/>
    public string? Type => Schema.Type;

    /// <summary>Gets the Properties.</summary>
    /// <remarks>
    /// Gets both the properties defined directly and via <see cref="AllOf"/>.
    /// </remarks>
    [Pure]
    public IReadOnlyList<ResolveOpenApiSchema> Properties =>
    [
        ..Schema.Properties.Select(p => new ResolveOpenApiSchema(Path.Child(p.Key), p.Value, Context, Model)),
        ..AllOf.Where(s => s.Path == Path).SelectMany(s => s.Properties),
    ];

    /// <inheritdoc cref="OpenApiSchema.Items"/>
    public ResolveOpenApiSchema? Items =>
        Schema.Items is { } items
        ? new(Path, items, Context, Model)
        : null;

    /// <summary>Returns true if the schema path matches the pattern.</summary>
    /// <remarks>
    /// Wildcards are supported.
    /// </remarks>
    [Pure]
    public bool Matches(string pattern, StringComparison comparisonType = StringComparison.OrdinalIgnoreCase)
    {
        var paths = Path.ToString().Split(OpenApiPath.Splitter);
        var patterns = pattern?.Split(OpenApiPath.Splitter) ?? [];

        if (patterns.Length > 0 && patterns.Length <= paths.Length)
        {
            for (var i = 1; i <= patterns.Length; i++)
            {
                if (!WildcardPattern.IsMatch(patterns[^i], paths[^i], default, comparisonType))
                {
                    return false;
                }
            }
            return true;
        }
        else return false;
    }

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

    [Pure]
    private ResolveOpenApiSchema SelectionOf(OpenApiSchema schema)
        => schema.Reference is { } r
        ? new ResolveOpenApiSchema(new(r.ReferenceV3 ?? r.ReferenceV2), schema, Context, null)
        : new(Path, schema, Context, Model);
}
