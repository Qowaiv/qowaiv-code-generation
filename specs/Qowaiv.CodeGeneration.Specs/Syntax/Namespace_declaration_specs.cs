namespace Syntax.Namespace_declaration_specs;

public class Content
{
    [Test]
    public void is_file_scoped_single_line()
        => ((Namespace)"Qowaiv.CodeGeneration.Syntax").Declaration()
        .Should().HaveContent("namespace Qowaiv.CodeGeneration.Syntax;");
}
