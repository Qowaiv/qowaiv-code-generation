using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.Code_snippet_specs;

public class Content
{
    private readonly CSharpWriterSettings Settings = new() { NewLine = "\n" };

    [TestCase("\n")]
    [TestCase("\r\n")]
    public void is_written_with_prevered_line_ending(string eol)
    {
        var settings = new CSharpWriterSettings { NewLine = eol };
        var snippet = CodeSnippet.Parse("public class Dummy\n{\r\n}\n");
        snippet.Should().HaveContent("public class Dummy\n{\n}", settings);
    }

    [Test]
    public void executes_if_when_constant_is_provided()
    {
        var snippet = CodeSnippet.Parse(@"-
#if PAR // exec
 with PAR
#else // exec
 without PAR
#endif // exec");

        var transformed = snippet.Transform([new Constant("PAR")]);
        transformed.Should().HaveContent("-\n with PAR", Settings);
    }

    [Test]
    public void executes_else_when_constant_is_not_provided()
    {
        var snippet = CodeSnippet.Parse(@"-
#if PAR // exec
 with PAR
#else // exec
 without PAR
#endif // exec");

        var transformed = snippet.Transform([]);
        transformed.Should().HaveContent("-\n without PAR", Settings);
    }

    [Test]
    public void executes_if_not_when_constant_is_not_provided()
    {
        var snippet = CodeSnippet.Parse(@"-
#if !PAR // exec
 without PAR
#else // exec
 with PAR
#endif // exec");

        var transformed = snippet.Transform([new Constant("PAR")]);
        transformed.Should().HaveContent("-\n with PAR", Settings);
    }

    [Test]
    public void executes_else_for_if_not_when_constant_is_provided()
    {
        var snippet = CodeSnippet.Parse(@"-
#if !PAR // exec
 without PAR
#else // exec
 with PAR
#endif // exec");

        var transformed = snippet.Transform([]);
        transformed.Should().HaveContent("-\n without PAR", Settings);
    }

    [Test]
    public void must_have_closing_endif()
    {
        var snippet = CodeSnippet.Parse("#if CONSTANT // exec");

        snippet.Invoking(s => s.Transform([]))
            .Should().Throw<ParseError>()
            .WithMessage("* Missing closing #endif statement.");
    }

}
