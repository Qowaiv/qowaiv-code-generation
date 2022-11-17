using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.Code_snippet_specs;

public class Content
{
    [TestCase("\n")]
    [TestCase("\r\n")]
    public void is_written_with_prevered_line_ending(string eol)
    {
        var settings = new CSharpWriterSettings { NewLine = eol };
        var snippet = new CodeSnippet("public class Dummy\n{\r\n}\n");
        snippet.Should().HaveContent(
            string.Join(eol, new[]
            {
                "public class Dummy",
                "{",
                "}",
                ""
            }), settings);
    }
}
