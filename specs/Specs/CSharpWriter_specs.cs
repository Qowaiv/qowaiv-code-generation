using System.IO;

namespace CSharpWriter_specs;

public class Namespaces
{
    [Test]
    public void supports_file_scoped()
    {
        using var text = new StringWriter();
        var writer = new CSharpWriter(text, new() { UseFileScopedNamespaces = true, NewLine = "\n" });

        using (writer.NamespaceDeclaration("Qowaiv.CodeGeneration"))
        {
            writer.Indent().Line("// not indented.");
        }

        text.ToString().Should().Be("namespace Qowaiv.CodeGeneration;\n\n// not indented.\n");
    }
    [Test]
    public void supports_pre_CSharp_10()
    {
        using var text = new StringWriter();
        var writer = new CSharpWriter(text, new() { UseFileScopedNamespaces = false, NewLine = "\n" });

        using (writer.NamespaceDeclaration("Qowaiv.CodeGeneration"))
        {
            writer.Indent().Line("// indented.");
        }

        text.ToString().Should().Be("namespace Qowaiv.CodeGeneration\n{\n    // indented.\n}\n");
    }
}
