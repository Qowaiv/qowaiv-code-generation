using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

public class EnumerationField : FieldInfo, Code
{
    public EnumerationField(Type enumType, string name, object? value)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(name));
        Value = value;
        FieldType = enumType;
    }

    /// <inheritdoc />
    public override FieldAttributes Attributes => default;

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
