using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.Text.RegularExpressions;

namespace Qowaiv.OpenApi;

public class OpenApiNamedSchema
{
    public OpenApiNamedSchema(string name, OpenApiSchema schema) 
    {
        Schema = Guard.NotNull(schema, nameof(schema));
        Name = Guard.NotNullOrEmpty(name, nameof(name));
    }

    public OpenApiSchema Schema { get; }

    public string Name { get; }

    /// <summary>
    /// Follow JSON Schema definition. Short text providing information about the data.
    /// </summary>
    public string? Title => Schema.Title;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// Value MUST be a string. Multiple types via an array are not supported.
    /// </summary>
    public OpenApiType Type
        => System.Enum.TryParse<OpenApiType>(Schema.Type, out var type)
        ? type
        : OpenApiType.None;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// While relying on JSON Schema's defined formats,
    /// the OAS offers a few additional predefined formats.
    /// </summary>
    public string? Format => Schema.Format;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// CommonMark syntax MAY be used for rich text representation.
    /// </summary>
    public string? Description => Schema.Description;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// This string SHOULD be a valid regular expression, according to the ECMA 262 regular expression dialect
    /// </summary>
    public Regex? Pattern
    {
        get
        {
            try
            {
                return Schema.Pattern is { }
                ? new Regex(Schema.Pattern)
                : null;
            }
            catch { return null; }
        }
    }

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// </summary>
    public int? MinLength => Schema.MinLength;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// </summary>
    public int? MaxLength => Schema.MaxLength;

    /// <summary>
    /// Allows sending a null value for the defined schema. Default value is false.
    /// </summary>
    public bool Nullable => Schema.Nullable;

    public int? MinItems => Schema.MinItems;
    public int? MaxItems => Schema.MaxItems;
    public Range ItemRange => new(start: MinItems ?? 0, end: MaxItems ?? int.MaxValue);

    /// <summary>
    /// A free-form property to include an example of an instance for this schema.
    /// To represent examples that cannot be naturally represented in JSON or YAML,
    /// a string value can be used to contain the example with escaping where necessary.
    /// </summary>
    public IOpenApiAny? Example => Schema.Example;

    /// <summary>
    /// Follow JSON Schema definition: https://tools.ietf.org/html/draft-fge-json-schema-validation-00
    /// </summary>
    public IList<IOpenApiAny> Enum => Schema.Enum;

    public OpenApiNamedSchema? Items 
        => Schema.Items is { } && Schema.Items.Reference is { }
        ? new(Schema.Items.Reference.Id, Schema.Items)
        : null;
    public IEnumerable<OpenApiNamedSchema> Properties 
        => Schema.Properties?
            .Where(kvp => !kvp.Value.AnyOf.Any())
            .Select(kvp => new OpenApiNamedSchema(kvp.Key, kvp.Value))
        ?? Array.Empty<OpenApiNamedSchema>();
}

