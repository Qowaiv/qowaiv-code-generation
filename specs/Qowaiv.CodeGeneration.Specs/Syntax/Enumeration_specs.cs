using Qowaiv.CodeGeneration.Syntax;

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


