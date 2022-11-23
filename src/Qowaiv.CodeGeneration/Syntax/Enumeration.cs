namespace Qowaiv.CodeGeneration.Syntax;

[Inheritable]
public class Enumeration : TypeBase, Code
{
    public Enumeration(TypeInfo info) : base(info) { }

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => null;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));

        writer.Write(new NamespaceDeclaration(NameSpace)).Line();

        foreach (var attr in AttributeInfos) writer.Write(attr);
        writer.Indent().Write("public enum ").Line(Name);

        using (writer.CodeBlock())
        {
            writer.Write(
                writes: Fields.OfType<Code>().Select(WriteField),
                split: writer => writer.Line(','))
                .Line();
        }

        Action<CSharpWriter> WriteField(Code field) => writer => writer.Write(field);
    }
}
