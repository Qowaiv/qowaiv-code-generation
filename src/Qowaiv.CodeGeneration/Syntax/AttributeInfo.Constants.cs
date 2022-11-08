namespace Qowaiv.CodeGeneration.Syntax;

public partial class AttributeInfo
{
    public static readonly AttributeInfo Newtonsoft_Json_Serialization_JsonConstructor = new(typeof(Newtonsoft.Json.JsonConstructorAttribute));

    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Any = new(typeof(Qowaiv.Validation.DataAnnotations.AnyAttribute));
    
    public static readonly AttributeInfo Qowaiv_Validation_DataAnnotations_Optional = new(typeof(Qowaiv.Validation.DataAnnotations.OptionalAttribute));
    
    public static readonly AttributeInfo System_ComponentModel_DataAnnotations_Required = new(typeof(System.ComponentModel.DataAnnotations.RequiredAttribute));

    public static readonly AttributeInfo System_Flags = new(typeof(System.FlagsAttribute));

    public static readonly AttributeInfo System_Text_Json_Serialization_JsonConstructor = new(typeof(System.Text.Json.Serialization.JsonConstructorAttribute));

    [Pure]
    public static AttributeInfo Newtonsoft_Json_Serialization_JsonProperty(string name) => new(typeof(Newtonsoft.Json.JsonPropertyAttribute), new[] { name });

    [Pure]
    public static AttributeInfo System_ComponentModel_DataAnnotations_RegularExpression(string pattern) => new(typeof(System.ComponentModel.DataAnnotations.RegularExpressionAttribute), new[] { pattern });

    [Pure]
    public static AttributeInfo System_Text_Json_Serialization_JsonPropertyName(string name) => new(typeof(System.Text.Json.Serialization.JsonPropertyNameAttribute), new[] { name });
}
