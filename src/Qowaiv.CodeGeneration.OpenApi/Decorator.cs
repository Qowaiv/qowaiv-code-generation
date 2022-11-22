using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

public class Decorator
{
    /// <summary>Decorate with System.Text.Json attributes (enabled by default).</summary>
    public bool SystemTextJson { get; init; } = true;

    /// <summary>Decorate with Newtonsoft.Json attributes (disabled by default).</summary>
    public bool NewtonsoftJson { get; init; } = false;

    [Pure]
    public virtual IEnumerable<AttributeInfo> Property(Property property, OpenApiProperty schema)
    {
        if (SystemTextJson) yield return AttributeInfo.System_Text_Json_Serialization_JsonPropertyName(schema.Name);
        if (NewtonsoftJson) yield return AttributeInfo.Newtonsoft_Json_Serialization_JsonProperty(schema.Name);
        if (PropertyRequired(property, schema) is { } required) yield return required;
        if (PropertyPattern(property, schema) is { } pattern) yield return pattern;
    }

    [Pure]
    protected virtual AttributeInfo? PropertyRequired(Property property, OpenApiProperty schema)
    {
        if (!schema.Schema.Nullable)
        {
            return property.PropertyType.IsArray
                ? AttributeInfo.Qowaiv_Validation_DataAnnotations_Any
                : AttributeInfo.System_ComponentModel_DataAnnotations_Required;
        }
        else return AttributeInfo.Qowaiv_Validation_DataAnnotations_Optional;
    }

    [Pure]
    protected virtual AttributeInfo? PropertyPattern(Property property, OpenApiProperty schema)
        => property.PropertyType == typeof(string) && schema.Schema.Pattern is { } pattern
        ? AttributeInfo.System_ComponentModel_DataAnnotations_RegularExpression(pattern)
        : null;
}
