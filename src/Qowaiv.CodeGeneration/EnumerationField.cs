using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents the <see cref="FieldInfo"/> of a enum value.</summary>
public sealed class EnumerationField : FieldInfo
{
    public EnumerationField(Type enumType, string name, string display, object? value)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(name));
        Value = value;
        FieldType = enumType;
    }

    public override FieldAttributes Attributes => throw new NotImplementedException();

    public override RuntimeFieldHandle FieldHandle => throw new NotImplementedException();

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
    [Pure]
    public override string ToString() => Value is { } ? $"{Name} = {Value}" : $"{Name}";

    public override object[] GetCustomAttributes(bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override object? GetValue(object? obj)
    {
        throw new NotImplementedException();
    }

    public override bool IsDefined(Type attributeType, bool inherit)
    {
        throw new NotImplementedException();
    }

    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, CultureInfo? culture)
    {
        throw new NotImplementedException();
    }
}
