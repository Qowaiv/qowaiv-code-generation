namespace Qowaiv.CodeGeneration.Syntax;

public partial class AttributeInfo
{
    public static class Newtonsoft
    {
        public static class Json
        {
            /// <summary><see cref="global::Newtonsoft.Json.JsonConstructorAttribute"/>.</summary>
            public static readonly AttributeInfo JsonConstructor = new(typeof(global::Newtonsoft.Json.JsonConstructorAttribute));

            /// <summary><see cref="global::Newtonsoft.Json.JsonPropertyAttribute"/>.</summary>
            [Pure]
            public static AttributeInfo JsonProperty(string name) => new(typeof(global::Newtonsoft.Json.JsonPropertyAttribute), [name]);
        }
    }

    public static class Qowaiv
    {
        public static class Diagnostics
        {
            public static class Contracts
            {
                /// <summary><see cref="global::Qowaiv.Diagnostics.Contracts.InheritableAttribute"/>.</summary>
                public static readonly AttributeInfo Inheritable = new(typeof(global::Qowaiv.Diagnostics.Contracts.InheritableAttribute));
            }
        }

        public static class Validation
        {
            public static class DataAnnotations
            {
                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.AnyAttribute"/>.</summary>
                public static readonly AttributeInfo Any = new(typeof(global::Qowaiv.Validation.DataAnnotations.AnyAttribute));

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.IsFiniteAttribute"/>.</summary>
                public static readonly AttributeInfo IsFinate = new(typeof(global::Qowaiv.Validation.DataAnnotations.IsFiniteAttribute));

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.MandatoryAttribute"/>.</summary>
                public static readonly AttributeInfo Mandatory = new(typeof(global::Qowaiv.Validation.DataAnnotations.MandatoryAttribute));

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.OptionalAttribute"/>.</summary>
                public static readonly AttributeInfo Optional = new(typeof(global::Qowaiv.Validation.DataAnnotations.OptionalAttribute));

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.AllowedAttribute{TValue}"/>.</summary>
                [Pure]
                public static AttributeInfo Allowed(Type propertyType, params object[] values)
                    => new(typeof(global::Qowaiv.Validation.DataAnnotations.AllowedAttribute<>).MakeGenericType(propertyType), values);

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.AllowedValuesAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo AllowedValues(params object[] values)
                    => new(typeof(global::Qowaiv.Validation.DataAnnotations.AllowedValuesAttribute), values);

                /// <summary><see cref="global::Qowaiv.Validation.DataAnnotations.MultipleOfAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo MultipleOf(double factor)
                    => new(typeof(global::Qowaiv.Validation.DataAnnotations.MultipleOfAttribute), [factor]);
            }
        }
    }

    public static class System
    {
        /// <summary><see cref="global::System.FlagsAttribute"/>.</summary>
        public static readonly AttributeInfo Flags = new(typeof(global::System.FlagsAttribute));

        public static class ComponentModel
        {
            public static class DataAnnotations
            {
                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.KeyAttribute"/>.</summary>
                public static readonly AttributeInfo Key = new(typeof(global::System.ComponentModel.DataAnnotations.KeyAttribute));

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.RequiredAttribute"/>.</summary>
                public static readonly AttributeInfo Required = new(typeof(global::System.ComponentModel.DataAnnotations.RequiredAttribute));

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.LengthAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo Length(int min, int max)
                    => new(typeof(global::System.ComponentModel.DataAnnotations.LengthAttribute), [min, max]);

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.MinLengthAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo MinLength(int min)
                    => new(typeof(global::System.ComponentModel.DataAnnotations.MinLengthAttribute), [min]);

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.MaxLengthAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo MaxLength(int max)
                    => new(typeof(global::System.ComponentModel.DataAnnotations.MaxLengthAttribute), [max]);

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.RangeAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo Range(double? min, double? max)
                    => new(typeof(global::System.ComponentModel.DataAnnotations.RangeAttribute), [min ?? double.MinValue, max ?? double.MaxValue]);

                /// <summary><see cref="global::System.ComponentModel.DataAnnotations.RegularExpressionAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo RegularExpression(string pattern)
                    => new(typeof(global::System.ComponentModel.DataAnnotations.RegularExpressionAttribute), [pattern]);
            }
        }

        public static class Runtime
        {
            public static class Serialization
            {
                /// <summary><see cref="global::System.Runtime.Serialization.EnumMemberAttribute"/>.</summary>
                [Pure]
                public static AttributeInfo EnumMember(string? value)
                    => new(typeof(global::System.Runtime.Serialization.EnumMemberAttribute), propertyValues: new { Value = (object?)value ?? Nill.Value });
            }
        }

        public static class Text
        {
            public static class Json
            {
                public static class Serialization
                {
                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonConstructorAttribute"/>.</summary>
                    public static readonly AttributeInfo JsonConstructor = new(typeof(global::System.Text.Json.Serialization.JsonConstructorAttribute));

                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonPolymorphicAttribute"/>.</summary>
                    [Pure]
                    public static AttributeInfo JsonPolymorphic(
                        string name,
                        global::System.Text.Json.Serialization.JsonUnknownDerivedTypeHandling? handling = default,
                        bool? ignoreUnrecognized = false) => new(
                            typeof(global::System.Text.Json.Serialization.JsonPolymorphicAttribute),
                            [name],
                            new
                            {
                                UnknownDerivedTypeHandling = handling,
                                IgnoreUnrecognizedTypeDiscriminators = ignoreUnrecognized,
                            });

                    /// <inheritdoc cref="global::System.Text.Json.Serialization.JsonConverterAttribute(Type)"/>
                    [Pure]
                    public static AttributeInfo JsonConverter<TConverter>() where TConverter : global::System.Text.Json.Serialization.JsonConverter
                        => JsonConverter(typeof(TConverter));

                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonConverterAttribute"/>.</summary>
                    [Pure]
                    public static AttributeInfo JsonConverter(Type type)
                        => new(typeof(global::System.Text.Json.Serialization.JsonConverterAttribute), [type]);

                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonDerivedTypeAttribute"/>.</summary>
                    [Pure]
                    public static AttributeInfo JsonDerivedType(Type type, string? discriminator = null) => new(
                        typeof(global::System.Text.Json.Serialization.JsonDerivedTypeAttribute),
                        discriminator is { } ? [type, discriminator] : [type]);

                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonDerivedTypeAttribute"/>.</summary>
                    [Pure]
                    public static AttributeInfo JsonIgnore(global::System.Text.Json.Serialization.JsonIgnoreCondition condition)
                        => new(typeof(global::System.Text.Json.Serialization.JsonIgnoreAttribute), propertyValues: new { Condition = condition });

                    /// <summary><see cref="global::System.Text.Json.Serialization.JsonPropertyNameAttribute"/>.</summary>
                    [Pure]
                    public static AttributeInfo JsonPropertyName(string name)
                        => new(typeof(global::System.Text.Json.Serialization.JsonPropertyNameAttribute), [name]);
                }
            }
        }
    }
}
