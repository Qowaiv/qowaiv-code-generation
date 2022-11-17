using Qowaiv.CodeGeneration.Syntax;

namespace Specs.Syntax.Literal_specs;

internal class Suports
{
    [TestCase(typeof(int), "typeof(int)")]
    [TestCase(null, "null")]
    [TestCase(true, "true")]
    [TestCase(false, "false")]
    [TestCase(@"Qowaiv ""quoted""", @"""Qowaiv \""quoted\""""")]
    [TestCase(typeof(Guid), "typeof(System.Guid)")]
    [TestCase(123d, "123d")]
    [TestCase(123.17, "123.17d")]
    public void Primitives(object? value, string content)
        => new Literal(value).Should().HaveContent(content);

    [TestCase(123, "123m")]
    [TestCase(123.17, "123.17m")]
    public void Decimals(decimal value, string content)
        => new Literal(value).Should().HaveContent(content);
}
