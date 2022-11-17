using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.OpenApi.Decorators;

public class DataAnnotationDecorator : CodeDecorator
{
    [Pure]
    public override IEnumerable<AttributeInfo> Property(Property property, OpenApiProperty schema)
    {
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

