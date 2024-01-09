namespace Qowaiv.CodeGeneration.Syntax;

public class Class : TypeBase, Code
{
    public Class(TypeInfo info) : base(info) { }

    protected virtual string Keyword => "class";

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => null;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        writer.Write(new NamespaceDeclaration(NameSpace)).Line();

        foreach (var attr in AttributeInfos) writer.Write(attr);

        writer.Indent().Write("public ");
        if (IsAbstract) writer.Write("abstract ");
        if (IsSealed) writer.Write("sealed ");
        if (IsPartial) writer.Write("partial ");
        writer.Write(Keyword).Write(' ').Write(Name);
        if (BaseType != typeof(object)) writer.Write(" : ").Write(BaseType);

        using (writer.Line().CodeBlock())
        {
            writer.Write(
                writes: Properties.OfType<Code>().Select(WriteProperty),
                split: writer => writer.Line());
        }

        Action<CSharpWriter> WriteProperty(Code property) => writer => writer.Write(property);
    }
}
