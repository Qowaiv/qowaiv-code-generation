namespace Qowaiv.CodeGeneration.Syntax;

public partial class AttributeInfo
{
    /// <summary><see cref="Newtonsoft.Json.JsonConstructorAttribute"/>.</summary>
    public static readonly AttributeInfo Newtonsoft_Json_JsonConstructor = new(typeof(Newtonsoft.Json.JsonConstructorAttribute));

    /// <summary><see cref="Qowaiv.Diagnostics.Contracts.InheritableAttribute"/>.</summary>
    public static readonly AttributeInfo Qowaiv_Diagnostics_Contracts_Inheritable = new(typeof(Qowaiv.Diagnostics.Contracts.InheritableAttribute));

    /// <summary><see cref="Qowaiv.Validation.DataAnnotations.AnyAttribute"/>.</summary>
    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Any = new(typeof(Qowaiv.Validation.DataAnnotations.AnyAttribute));

    /// <summary><see cref="Qowaiv.Validation.DataAnnotations.IsFiniteAttribute"/>.</summary>
    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_IsFinate = new(typeof(Qowaiv.Validation.DataAnnotations.IsFiniteAttribute));

    /// <summary><see cref="Qowaiv.Validation.DataAnnotations.MandatoryAttribute"/>.</summary>
    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Mandatory = new(typeof(Qowaiv.Validation.DataAnnotations.MandatoryAttribute));

    /// <summary><see cref="Qowaiv.Validation.DataAnnotations.OptionalAttribute"/>.</summary>
    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Optional = new(typeof(Qowaiv.Validation.DataAnnotations.OptionalAttribute));

    /// <summary><see cref="System.ComponentModel.DataAnnotations.KeyAttribute"/>.</summary>
    public static readonly AttributeInfo System_ComponentModel_DataAnnotations_Key = new(typeof(System.ComponentModel.DataAnnotations.KeyAttribute));

    /// <summary><see cref="System.ComponentModel.DataAnnotations.RequiredAttribute"/>.</summary>
    public static readonly AttributeInfo System_ComponentModel_DataAnnotations_Required = new(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute));

    /// <summary><see cref="System.FlagsAttribute"/>.</summary>
    public static readonly AttributeInfo System_Flags = new(typeof(System.FlagsAttribute));

    /// <summary><see cref="System.Text.Json.Serialization.JsonConstructorAttribute"/>.</summary>
    public static readonly AttributeInfo System_Text_Json_Serialization_JsonConstructor = new(typeof(System.Text.Json.Serialization.JsonConstructorAttribute));

    /// <summary><see cref="System.Text.Json.Serialization.JsonConstructorAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo Newtonsoft_Json_Serialization_JsonProperty(string name) => new(typeof(Newtonsoft.Json.JsonPropertyAttribute), [name]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.LengthAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_Length(int min, int max)
        => new(typeof(System.ComponentModel.DataAnnotations.LengthAttribute), [min, max]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.MinLengthAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_MinLength(int min)
        => new(typeof(System.ComponentModel.DataAnnotations.MinLengthAttribute), [min]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.MaxLengthAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_MaxLength(int max)
        => new(typeof(System.ComponentModel.DataAnnotations.MaxLengthAttribute), [max]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.RangeAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_Range(double? min, double? max)
        => new(typeof(System.ComponentModel.DataAnnotations.RangeAttribute), [min ?? double.MinValue, max ?? double.MaxValue]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.RegularExpressionAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_RegularExpression(string pattern)
        => new(typeof(System.ComponentModel.DataAnnotations.RegularExpressionAttribute), [pattern]);

    /// <summary><see cref="System.ComponentModel.DataAnnotations.AllowedValuesAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo Qowaiv_Validation_DataAnnotations_AllowedValues(params object[] values)
        => new(typeof(Qowaiv.Validation.DataAnnotations.AllowedValuesAttribute), values);

    /// <summary><see cref="Qowaiv.Validation.DataAnnotations.MultipleOfAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo Qowaiv_Validation_DataAnnotations_MultipleOf(double factor)
        => new(typeof(Qowaiv.Validation.DataAnnotations.MultipleOfAttribute), [factor]);

    /// <summary><see cref="System.Text.Json.Serialization.JsonPolymorphicAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Text_Json_SerializationJsonPolymorphic(
        string name,
        System.Text.Json.Serialization.JsonUnknownDerivedTypeHandling? handling = default,
        bool? ignoreUnrecognized = false) => new(
            typeof(System.Text.Json.Serialization.JsonPolymorphicAttribute),
            [name],
            new
            {
                UnknownDerivedTypeHandling = handling,
                IgnoreUnrecognizedTypeDiscriminators = ignoreUnrecognized,
            });

    /// <inheritdoc cref="System_Text_Json_Serialization_JsonConverter(Type)"/>
    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonConverter<TConverter>() where TConverter : System.Text.Json.Serialization.JsonConverter
        => System_Text_Json_Serialization_JsonConverter(typeof(TConverter));

    /// <summary><see cref="System.Text.Json.Serialization.JsonConverterAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonConverter(Type type)
        => new(typeof(System.Text.Json.Serialization.JsonConverterAttribute), [type]);

    /// <summary><see cref="System.Text.Json.Serialization.JsonDerivedTypeAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonDerivedType(Type type, string? discriminator = null) => new(
        typeof(System.Text.Json.Serialization.JsonDerivedTypeAttribute),
        discriminator is { } ? [type, discriminator] : [type]);

    /// <summary><see cref="System.Text.Json.Serialization.JsonDerivedTypeAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonIgnore(System.Text.Json.Serialization.JsonIgnoreCondition condition)
        => new(typeof(System.Text.Json.Serialization.JsonIgnoreAttribute), null, new { Condition = condition });

    /// <summary><see cref="System.Text.Json.Serialization.JsonPropertyNameAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonPropertyName(string name)
        => new(typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), [name]);

    /// <summary><see cref="System.Runtime.Serialization.EnumMemberAttribute"/>.</summary>
    [Pure]
    public static AttributeInfo System_Runtime_Serialization_EnumMember(string? value)
        => new(typeof(System.Runtime.Serialization.EnumMemberAttribute), null, new { Value = (object?)value ?? Nill.Value });
}
