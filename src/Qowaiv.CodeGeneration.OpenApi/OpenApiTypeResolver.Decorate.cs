using Microsoft.OpenApi.Any;
using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    [Pure]
    protected virtual IEnumerable<AttributeInfo> DecorateEnumerationField(EnumerationField field, OpenApiString schema)
    {
        yield return AttributeInfo.System_Runtime_Serialization_EnumMember(Guard.NotNull(schema).Value);
    }

    [Pure]
    public virtual IEnumerable<AttributeInfo> DecorateModel(Class @class, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(@class);

        foreach (var derived in @class.GetDerivedTypes().Select(t => DecorateDerivedType(t, schema)).OfType<AttributeInfo>())
        {
            yield return derived;
        }
    }

    [Pure]
    protected virtual AttributeInfo? DecorateDerivedType(Type derivedType, ResolveOpenApiSchema schema)
    {
        return AttributeInfo.System_Text_Json_Serialization_JsonDerivedTypeAttribute(derivedType);
    }

    [Pure]
    protected virtual IEnumerable<AttributeInfo> DecorateProperty(Property property, ResolveOpenApiSchema schema)
    {
        if (DecoratePropertyJson(property, schema) is { } json) yield return json;
        if (DecoratePropertyRequired(property, schema) is { } required) yield return required;
        if (DecoratePropertyPattern(property, schema) is { } pattern) yield return pattern;
        if (PropertyPropertyMultipleOf(property, schema) is { } multiple) yield return multiple;
        if (DecoratePropertyAllowedValues(property, schema) is { } allowed) yield return allowed;
        if (DecoratePropertyMinLength(property, schema) is { } min) yield return min;
        if (DecoratePropertyMaxLength(property, schema) is { } max) yield return max;
        if (DecoratePropertyRange(property, schema) is { } range) yield return range;
        if (DecorateIsFinate(property, schema) is { } finate) yield return finate;
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyJson(Property property, ResolveOpenApiSchema schema)
    {
        return AttributeInfo.System_Text_Json_Serialization_JsonPropertyName(schema.Path.Last);
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyRequired(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        if (property.IsRequired)
        {
            if (property.PropertyType.IsArray)
            {
                return AttributeInfo.Qowaiv_Validation_DataAnnotations_Any;
            }
            else if (!property.PropertyType.IsNullableValueType() && IsNullableByDesign(property.PropertyType))
            {
                return AttributeInfo.Qowaiv_Validation_DataAnnotations_Mandatory;
            }
            else return AttributeInfo.System_ComponentModel_DataAnnotations_Required;
        }
        else return AttributeInfo.Qowaiv_Validation_DataAnnotations_Optional;
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyPattern(Property property, ResolveOpenApiSchema schema)
        => Guard.NotNull(property).PropertyType == typeof(string) && schema.Schema.Pattern is { } pattern
            ? AttributeInfo.System_ComponentModel_DataAnnotations_RegularExpression(pattern)
            : null;

    [Pure]
    protected virtual AttributeInfo? PropertyPropertyMultipleOf(Property property, ResolveOpenApiSchema schema)
       => schema.Schema.MultipleOf is { } factor
       ? AttributeInfo.Qowaiv_Validation_DataAnnotations_MultipleOf((double)factor)
       : null;

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyMinLength(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        if (schema.MinLength is { } min)
        {
            return AttributeInfo.System_ComponentModel_DataAnnotations_MinLength(min);
        }
        else return null;
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyMaxLength(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        if (schema.MaxLength is { } max)
        {
            return AttributeInfo.System_ComponentModel_DataAnnotations_MaxLength(max);
        }
        else return null;
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyRange(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        if (schema.Minimum is { } || schema.Maximum is { })
        {
            return AttributeInfo.System_ComponentModel_DataAnnotations_Range((double?)schema.Minimum, (double?)schema.Maximum);
        }
        else return null;
    }

    [Pure]
    protected virtual AttributeInfo? DecorateIsFinate(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        var type = QowaivType.GetNotNullableType(property.PropertyType);

        if (type == typeof(double) || type == typeof(float))
        {
            return AttributeInfo.Qowaiv_Validation_DataAnnotations_IsFinate;
        }
        else return null;
    }

    [Pure]
    protected virtual AttributeInfo? DecoratePropertyAllowedValues(Property property, ResolveOpenApiSchema schema)
    {
        Guard.NotNull(property);

        var nt = Reflection.QowaivType.GetNotNullableType(property.PropertyType);

        if (IsEnumWithOnlyOpenApiStrings(schema)
            && (!nt.IsEnum || OnlySubsetIsAllowed(nt, schema)))
        {
            var values = schema.Enum
                .Cast<OpenApiString>()
                .Select(str => str.Value)
                .ToArray();

            return AttributeInfo.Qowaiv_Validation_DataAnnotations_AllowedValues(values);
        }
        else return null;

        static bool IsEnumWithOnlyOpenApiStrings(ResolveOpenApiSchema schema) 
            => schema.Enum.Any() && schema.Enum.All(val => val is OpenApiString);

        static bool OnlySubsetIsAllowed(Type type, ResolveOpenApiSchema schema)
            => type is Enumeration enumeration
            && enumeration
                .GetFields()
                .OfType<EnumerationField>()
                .Count(f => f.Name != "None" && !0.Equals(f.Value)) > schema.Enum.Count;
    }
}
