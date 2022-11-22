using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

[Inheritable]
public class Class : ObjectBase, Code
{
    public Class(
        TypeName type,
        IReadOnlyCollection<AttributeInfo>? attributes = null,
        IReadOnlyCollection<ConstructorInfo>? constructors = null,
        IReadOnlyCollection<EventInfo>? events = null,
        IReadOnlyCollection<FieldInfo>? fields = null,
        IReadOnlyCollection<MethodInfo>? methods = null,
        IReadOnlyCollection<PropertyInfo>? properties = null,
        IReadOnlyCollection<Type>? interfaces = null,
        Type? baseType = null) : base(type, baseType, attributes, constructors, events, fields, methods, properties, interfaces) { }

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType()
        => IsArray
        ? new Class(TypeName, AttributeInfos, Constructors, Events, Fields, Methods, Properties, Interfaces, BaseType)
        : null;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));

        writer.Write(new NamespaceDeclaration(NameSpace)).Line();

        foreach (var attr in AttributeInfos) writer.Write(attr);

        writer.Indent().Write("public ");
        if (IsAbstract) writer.Write("abstract ");
        if (IsSealed) writer.Write("sealed ");
        writer.Write("partial class ").Write(Name);
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
