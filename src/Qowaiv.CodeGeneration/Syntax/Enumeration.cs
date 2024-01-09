namespace Qowaiv.CodeGeneration.Syntax;

public class Enumeration : ObjectBase
{
    public Enumeration(
        TypeName type,
        IReadOnlyCollection<EnumerationField> fields,
        IReadOnlyCollection<AttributeInfo>? attributes = null,
        bool isArray = false)
        : base(type: type,
            baseType: typeof(Enum),
            isArray: isArray,
            attributes: attributes,
            constructors: null,
            events: null,
            fields: fields,
            methods: null,
            properties: null,
            interfaces: null)
    { }

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType()
        => IsArray
        ? new Enumeration(TypeName, Fields.OfType<EnumerationField>().ToArray(), AttributeInfos, isArray: false)
        : null;

    /// <inheritdoc />
    public override void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));

        writer.Write(new NamespaceDeclaration(NameSpace));

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
