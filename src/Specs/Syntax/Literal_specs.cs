using Qowaiv.CodeGeneration.Syntax;

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
    public void Decimal() => new Literal(42m).ToString().Should().Be("42m");

    [Test]
    public void Double() => new Literal(3.14).ToString().Should().Be("3.14");

    [Test]
    public void String() => new Literal("Hello, world!").ToString().Should().Be("\"Hello, world!\"");
}
