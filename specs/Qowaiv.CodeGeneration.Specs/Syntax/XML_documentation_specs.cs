using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.XML_documentation_specs;

public class XML_escapes
{
    [Test]
    public void Summary()
    {
        var comment = new XmlDocumentation { Summary = "Pride & Joy" };

        comment.Should().HaveContent("/// <summary>Pride &amp; Joy</summary>\n");
    }

    [Test]
    public void Multiline_summary()
    {
        var comment = new XmlDocumentation { Summary = "Pride & Joy\n666 <> the devil" };

        comment.Should().HaveContent(@"/// <summary>
/// Pride &amp; Joy
/// 666 &lt;&gt; the devil
/// </summary>
");
    }

    [Test]
    public void Remarks()
    {
        var comment = new XmlDocumentation { Remarks = "Pride & Joy" };

        comment.Should().HaveContent("/// <remarks>Pride &amp; Joy</remarks>\n");
    }
}
