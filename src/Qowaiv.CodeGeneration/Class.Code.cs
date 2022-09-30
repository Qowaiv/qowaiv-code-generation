using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public partial class Class
{
    /// <inheritdoc />
    public override void WriteTo(CSharpWriter writer)
    {
        writer.NamespaceDeclaration(NS);

        foreach (var decoration in Decorations) writer.Write(decoration);

        writer.Indent().Write("public ");
        if (IsAbstract) writer.Write("abstract ");
        if (IsSealed) writer.Write("sealed ");
        writer.Write("partial class ").Write(Name);
        if (BaseType != typeof(object)) writer.Write(" : ").Write(BaseType);

        using (writer.Line().CodeBlock())
        {
            writer.Write(
                writes: Properties.Select(WriteProperty),
                split: writer => writer.Line());
        }

        Action<CSharpWriter> WriteProperty(Property property) => writer => writer.Write(property);
    }
}
