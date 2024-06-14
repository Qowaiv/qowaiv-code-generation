namespace Qowaiv.CodeGeneration;

public partial class AttributeInfo
{
    public static readonly AttributeInfo Newtonsoft_Json_Serialization_JsonConstructor = new(typeof(Newtonsoft.Json.JsonConstructorAttribute));

    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Any = new(typeof(Qowaiv.Validation.DataAnnotations.AnyAttribute));

    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Mandatory = new(typeof(Qowaiv.Validation.DataAnnotations.MandatoryAttribute));

    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Optional = new(typeof(Qowaiv.Validation.DataAnnotations.OptionalAttribute));

    public static readonly AttributeInfo System_ComponentModel_DataAnnotations_Key = new(typeof(System.ComponentModel.DataAnnotations.KeyAttribute));

    public static readonly AttributeInfo System_ComponentModel_DataAnnotations_Required = new(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute));

    public static readonly AttributeInfo System_Flags = new(typeof(System.FlagsAttribute));

    public static readonly AttributeInfo System_Text_Json_Serialization_JsonConstructor = new(typeof(System.Text.Json.Serialization.JsonConstructorAttribute));

    [Pure]
    public static AttributeInfo Newtonsoft_Json_Serialization_JsonProperty(string name) => new(typeof(Newtonsoft.Json.JsonPropertyAttribute), new[] { name });

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_Length(int min, int max)
        => new(typeof(System.ComponentModel.DataAnnotations.LengthAttribute), [min, max]);

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_MinLength(int min)
        => new(typeof(System.ComponentModel.DataAnnotations.MinLengthAttribute), [min]);

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_MaxLength(int max)
        => new(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), [max]);

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_Range(double? min, double? max)
        => new(typeof(System.ComponentModel.DataAnnotations.RangeAttribute), [min ?? double.MinValue, max ?? double.MaxValue]);

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_RegularExpression(string pattern)
        => new(typeof(System.ComponentModel.DataAnnotations.RegularExpressionAttribute), [pattern]);

    [Pure]
    public static AttributeInfo Qowaiv_Validation_DataAnnotations_AllowedValues(params object[] values)
        => new(typeof(Qowaiv.Validation.DataAnnotations.AllowedValuesAttribute), values);

    [Pure]
    public static AttributeInfo Qowaiv_Validation_DataAnnotations_MultipleOf(double factor)
        => new(typeof(Qowaiv.Validation.DataAnnotations.MultipleOfAttribute), [factor]);

    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonDerivedTypeAttribute(Type type)
        => new(typeof(System.Text.Json.Serialization.JsonDerivedTypeAttribute), [type]);

    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonPropertyName(string name)
        => new(typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), [name]);

    [Pure]
    public static AttributeInfo System_Runtime_Serialization_EnumMember(string? value)
        => new(typeof(System.Runtime.Serialization.EnumMemberAttribute), null, KeyValuePair.Create("Value", (object?)value));
}
