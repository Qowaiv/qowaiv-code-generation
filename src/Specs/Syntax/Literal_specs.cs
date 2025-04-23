using Qowaiv.CodeGeneration.Syntax;
using System;

namespace Specs.Syntax.Literal_specs;

public class Generates
{
    [Test]
    public void Null() => new Literal(null).ToString().Should().Be("null");

    [Test]
    public void Type() => new Literal(typeof(object)).ToString().Should().Be("typeof(object)");

    [Test]
    public void False() => new Literal(false).ToString().Should().Be("false");

    [Test]
    public void True() => new Literal(true).ToString().Should().Be("true");

    [Test]
    public void Int() => new Literal(42).ToString().Should().Be("42");

    [Test]
    public void Int_MinValue() => new Literal(int.MinValue).ToString().Should().Be("int.MinValue");
    [Test]
    public void Int_MaxValue() => new Literal(int.MaxValue).ToString().Should().Be("int.MaxValue");

    [Test]
    public void Decimal() => new Literal(42m).ToString().Should().Be("42m");

    [Test]
    public void Double() => new Literal(3.14).ToString().Should().Be("3.14");

    [Test]
    public void Double_MinValue() => new Literal(double.MinValue).ToString().Should().Be("double.MinValue");

    [Test]
    public void Double_MaxValue() => new Literal(double.MaxValue).ToString().Should().Be("double.MaxValue");
    
    [Test]
    public void Double_NaN() => new Literal(double.NaN).ToString().Should().Be("double.NaN");

    [Test]
    public void Double_NegativeInfinity() => new Literal(double.NegativeInfinity).ToString().Should().Be("double.NegativeInfinity");

    [Test]
    public void Double_PositiveInfinity() => new Literal(double.PositiveInfinity).ToString().Should().Be("double.PositiveInfinity");

    [Test]
    public void String() => new Literal("Hello, world!").ToString().Should().Be("\"Hello, world!\"");

    [Test]
    public void Enum() => new Literal(System.TypeCode.Empty).ToString().Should().Be("System.TypeCode.Empty");
}

public class Throws
{
    [Test]
    public void On_objects_lacking_literal_representation()
        => new object().Invoking(o => new Literal(o).ToString())
            .Should().Throw<NotSupportedException>()
            .WithMessage("Literals of type System.Object are not supported");
}
