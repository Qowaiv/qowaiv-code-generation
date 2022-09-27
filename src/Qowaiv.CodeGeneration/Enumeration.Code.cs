using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public partial class Enumeration
{
    /// <inheritdoc />
    public override void Write(CSharpWriter writer)
    {
        foreach (var decoration in Decorations) writer.Write(decoration);

        writer.NamespaceDeclaration(NS);
        writer.Indent().Write("public enum ").Line(Name);
        
        using (writer.CodeBlock())
        {
            writer.Write(
                writes: Fields.Select(WriteField),
                split: writer => writer.Line());
        }

        Action<CSharpWriter> WriteField(EnumerationField field) => writer => writer.Write(field.ToString());
    }
}
