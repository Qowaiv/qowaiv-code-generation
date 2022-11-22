namespace Qowaiv.CodeGeneration.Syntax;

[Inheritable]
public class Enumeration : ObjectBase, Code
{
    public Enumeration(
        TypeName type,
        IReadOnlyCollection<EnumerationField> fields,
        IReadOnlyCollection<AttributeInfo>? attributes = null)
        : base(type: type,
            baseType: typeof(Enum),
            attributes: attributes,
            constructors: null,
            events: null,
            fields: fields,
            methods: null,
            properties: null,
            interfaces: null) { }

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType()
        => IsArray
        ? new Enumeration(TypeName, Fields.OfType<EnumerationField>().ToArray(), AttributeInfos)
        : null;

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
