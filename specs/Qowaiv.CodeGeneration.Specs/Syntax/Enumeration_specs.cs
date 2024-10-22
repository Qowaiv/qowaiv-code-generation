using Qowaiv.CodeGeneration.Syntax;
using System.Xml.Serialization;

namespace Syntax.Enumeration_specs;

public class Equals_Enum_on
{
    internal static readonly Enumeration Enumeration = new(new() { TypeName = new("System", "TypeCode") });

    private static readonly Type Enum = typeof(System.TypeCode);

    [Test]
    public void Attributes()
       => Enumeration.Attributes.Should().Be(Enum.Attributes);

    [Test]
    public void DeclaringType()
        => Enumeration.DeclaringType.Should().Be(Enum.DeclaringType);

    [Test]
    public void IsValueType()
        => Enumeration.IsValueType.Should().Be(Enum.IsValueType);

    [Test]
    public void IsClass()
        => Enumeration.IsClass.Should().Be(Enum.IsClass);

    [Test]
    public void IsEnum()
        => Enumeration.IsEnum.Should().Be(Enum.IsEnum);

    [Test]
    public void IsInterface()
        => Enumeration.IsInterface.Should().Be(Enum.IsInterface);

    [Test]
    public void IsPrimitive()
        => Enumeration.IsPrimitive.Should().Be(Enum.IsPrimitive);

    [Test]
    public void IsSealed()
        => Enumeration.IsSealed.Should().Be(Enum.IsSealed);
}

public class Generates
{
    [Test]
    public void Content()
    {
        var fields = new List<EnumerationField>();
        var enumeration = new Enumeration(new() 
        {
            TypeName = new("System", "TypeCode"),
            Fields = fields,
            Attributes = [new AttributeInfo(typeof(SerializableAttribute))],
            Documentation = new() {  Summary = "Specifies the type of an object." },
        });
        fields.AddRange([
            new EnumerationField(enumeration, "None", 0, [new AttributeInfo(typeof(XmlEnumAttribute), [null])]),
            new EnumerationField(enumeration, "One", 1, [new AttributeInfo(typeof(XmlEnumAttribute), ["1"])]),
            new EnumerationField(enumeration, "Other", null, [new AttributeInfo(typeof(XmlEnumAttribute), ["x"])], new XmlDocumentation() { Summary = "Other options."}),
        ]);

        ((Code)enumeration).Should().HaveContent(@"namespace System;

/// <summary>Specifies the type of an object.</summary>
[System.Serializable]
public enum TypeCode
{
    [System.Xml.Serialization.XmlEnum(null)]
    None = 0,

    [System.Xml.Serialization.XmlEnum(""1"")]
    One = 1,

    /// <summary>Other options.</summary>
    [System.Xml.Serialization.XmlEnum(""x"")]
    Other
}
");
    }
}


