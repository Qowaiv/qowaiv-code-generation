namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a class.</summary>
public class Class : TypeBase, Code
{
    /// <summary>Initializes a new instance of the <see cref="Class"/> class.</summary>
    public Class(TypeInfo info) : base(info) { }

    /// <summary>The class keyword.</summary>
    protected virtual string Keyword => "class";

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => null;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        writer.Write(new NamespaceDeclaration(NameSpace)).Line();

        Info.Documentation?.WriteTo(writer);

        foreach (var attr in Info.Attributes) writer.Write(attr);

        writer.Indent().Write(Visibility).Write(' ');
        if (IsAbstract) writer.Write("abstract ");
        if (IsSealed) writer.Write("sealed ");
        if (IsPartial) writer.Write("partial ");
        writer.Write(Keyword).Write(' ').Write(Name);
        if (BaseType != typeof(object)) writer.Write(" : ").Write(BaseType);

        using (writer.Line().CodeBlock())
        {
            writer.Write(
                writes: Info.Properties.OfType<Code>().Select(WriteProperty),
                split: writer => writer.Line());
        }

        Action<CSharpWriter> WriteProperty(Code property) => writer => writer.Write(property);
    }
}
