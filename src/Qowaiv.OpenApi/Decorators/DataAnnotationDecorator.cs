using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.Instructions;
using Qowaiv.CodeGeneration.IO;
using Qowaiv.Validation.DataAnnotations;
using System.ComponentModel.DataAnnotations;

namespace Qowaiv.OpenApi.Decorators;

public class DataAnnotationDecorator : CodeDecorator
{
    [Pure]
    public override IEnumerable<Code> Property(Property property, OpenApiProperty schema)
    {
        if (PropertyRequired(property, schema) is { } required) yield return required;
        if (PropertyPattern(property, schema) is { } pattern) yield return pattern;
    }

    [Pure]
    protected virtual Code? PropertyRequired(Property property, OpenApiProperty schema)
    {
        if (!schema.Schema.Nullable)
        {
            return property.PropertyType.IsArray
                ? new Decoration(typeof(AnyAttribute))
                : new Decoration(typeof(RequiredAttribute));
        }
        else return new Decoration(typeof(OptionalAttribute));
    }

    [Pure]
    protected virtual Code? PropertyPattern(Property property, OpenApiProperty schema) 
        => property.PropertyType == TypeInfo.String && schema.Schema.Pattern is { } pattern
        ? new Decoration(typeof(RegularExpressionAttribute), new object[] { pattern })
        : null;
}

