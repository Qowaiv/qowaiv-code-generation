namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents an enum.</summary>
[Inheritable]
public class Enumeration : TypeBase, Code
{
    /// <summary>Initializes a new instance of the <see cref="Enumeration"/> class.</summary>
    public Enumeration(TypeInfo info) : base(Enrich(info)) { }

    [Pure]
    private static TypeInfo Enrich(TypeInfo info) => Guard.NotNull(info) with
    {
        BaseType = typeof(Enum),
        IsSealed = true,
        IsAbstract = false,
        IsStatic = false,
    };

    /// <inheritdoc />
    [Pure]
    public sealed override Type? GetElementType() => null;

    /// <inheritdoc />
    public sealed override Type BaseType => typeof(Enum);

    /// <inheritdoc />
    public sealed override bool IsEnum => true;

    /// <inheritdoc />
    [Pure]
    protected sealed override bool IsValueTypeImpl() => true;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        using (writer.NamespaceDeclaration(NameSpace))
        {
            Info.Documentation?.WriteTo(writer);

            foreach (var attr in Info.Attributes) writer.Write(attr);

            writer.Indent().Write(Visibility).Write(" enum ").Line(Name);

            using (writer.CodeBlock())
            {
                writer.Write(
                    writes: Info.Fields.OfType<Code>().Select(WriteField),
                    split: writer => writer.Line(',').Line())
                    .Line();
            }
        }
        Action<CSharpWriter> WriteField(Code field) => writer => writer.Write(field);
    }
}
