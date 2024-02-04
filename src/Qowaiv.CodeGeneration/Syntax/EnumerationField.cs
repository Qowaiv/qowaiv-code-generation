using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents the filed of an enum.</summary>
public class EnumerationField : FieldInfo, Code
{
    /// <summary>Collection of <see cref="AttributeInfo"/>s.</summary>
    protected readonly IReadOnlyCollection<Code> AttributeInfos;

    /// <summary>The (optional) documentation.</summary>
    protected readonly XmlDocumentation? Documentation;

    /// <summary>Initializes a new instance of the <see cref="EnumerationField"/> class.</summary>
    public EnumerationField(
        Type enumType,
        string name,
        object? value,
        IReadOnlyCollection<AttributeInfo>? attributes = null,
        XmlDocumentation? documentation = null)
    {
        Name = Guard.NotNullOrEmpty(name);
        Value = value;
        FieldType = enumType;
        AttributeInfos = attributes ?? Array.Empty<AttributeInfo>();
        Documentation = documentation;
    }

    /// <inheritdoc />
    public override FieldAttributes Attributes => FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault;

    /// <inheritdoc />
    public override RuntimeFieldHandle FieldHandle => default;

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>The (optional) value.</summary>
    public object? Value { get; }

    /// <inheritdoc />
    public override Type FieldType { get; }

    /// <inheritdoc />
    public override Type DeclaringType => FieldType;

    /// <inheritdoc />
    public override Type ReflectedType => FieldType;

    /// <inheritdoc />
    public virtual void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        Documentation?.WriteTo(writer);

        foreach (var decoration in AttributeInfos) writer.Write(decoration);

        writer.Indent().Write(Name);
        if (Value is { })
        {
            writer.Write(" = ").Write(new Literal(Value));
        }
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit) => Array.Empty<object>();

    /// <inheritdoc />
    [Pure]
    public override bool IsDefined(Type attributeType, bool inherit) => false;

    /// <inheritdoc />
    [Pure]
    public override object? GetValue(object? obj) => throw new NotSupportedException();

    /// <inheritdoc />
    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture) => throw new NotSupportedException();
}
