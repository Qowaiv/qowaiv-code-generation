using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a class.</summary>
public class Class : TypeBase, Code
{
    /// <summary>Initializes a new instance of the <see cref="Class"/> class.</summary>
    public Class(TypeData data) : base(data) { }

    /// <summary>The class keyword.</summary>
    protected virtual string Keyword => "class";

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => null;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        using (writer.NamespaceDeclaration(NameSpace))
        {
            Data.Documentation?.WriteTo(writer);

            foreach (var attr in Data.Attributes) writer.Write(attr);

            writer.Indent().Write(Visibility).Write(' ');
            if (IsAbstract) writer.Write("abstract ");
            if (IsSealed) writer.Write("sealed ");
            if (IsPartial) writer.Write("partial ");
            writer.Write(Keyword).Write(' ').Write(Name);
            if (BaseType != typeof(object)) writer.Write(" : ").Write(BaseType);

            using (writer.Line().CodeBlock())
            {
                writer.Write(
                    writes: Data.Properties
                    .Where(NotDefinedByBase)
                    .OfType<Code>()
                    .Select(WriteProperty),
                    split: writer => writer.Line());
            }
        }

        Action<CSharpWriter> WriteProperty(Code property) => writer => writer.Write(property);
    }

    /// <summary>True if the property is not defined by any base type.</summary>
    [Pure]
    private bool NotDefinedByBase(PropertyInfo info)
    {
        if (info.DeclaringType == this) return true;

        Type? @base = BaseType;

        while (@base is { })
        {
            if (info.DeclaringType == @base) return false;

            @base = @base.BaseType;
        }
        return true;
    }
}
