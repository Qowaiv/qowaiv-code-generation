using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

public class EnumerationField : FieldInfo, Code
{
    protected readonly IReadOnlyCollection<Code> AttributeInfos;

    public EnumerationField(
        Type enumType,
        string name,
        object? value,
        IReadOnlyCollection<AttributeInfo>? attributes = null)
    {
        Name = Guard.NotNullOrEmpty(name);
        Value = value;
        FieldType = enumType;
        AttributeInfos = attributes ?? Array.Empty<AttributeInfo>();
    }

    /// <inheritdoc />
    public override FieldAttributes Attributes => FieldAttributes.Public | FieldAttributes.Static | FieldAttributes.Literal | FieldAttributes.HasDefault;

    /// <inheritdoc />
    public override RuntimeFieldHandle FieldHandle => default;

    /// <inheritdoc />
    public override string Name { get; }

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
    [Pure]
    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture) => throw new NotSupportedException();
}
