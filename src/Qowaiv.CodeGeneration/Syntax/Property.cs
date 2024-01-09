using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents the <see cref="PropertyInfo"/> of a <see cref="Type"/> property.</summary>
public partial class Property : PropertyInfo, Code
{
    protected readonly IReadOnlyCollection<Code> AttributeInfos;

    public Property(
        string name,
        Type propertyType,
        Type declaringClass,
        PropertyAccess access,
        IReadOnlyCollection<AttributeInfo>? attributes = null)
    {
        Name = Guard.NotNullOrEmpty(name, nameof(name));
        PropertyType = Guard.NotNull(propertyType, nameof(propertyType));
        DeclaringType = Guard.NotNull(declaringClass, nameof(declaringClass));
        PropertyAccess = Guard.DefinedEnum(access, nameof(access));
        AttributeInfos = attributes ?? Array.Empty<AttributeInfo>();
    }

    /// <inheritdoc />
    public override string Name { get; }

    /// <inheritdoc />
    public override Type PropertyType { get; }

    /// <inheritdoc />
    public override Type DeclaringType { get; }

    public PropertyAccess PropertyAccess { get; }

    /// <inheritdoc />
    public override bool CanRead => true;

    /// <inheritdoc />
    public override bool CanWrite => PropertyAccess != PropertyAccess.GetOnly;

    /// <inheritdoc />
    public override Type ReflectedType => DeclaringType;

    /// <inheritdoc />
    public override PropertyAttributes Attributes => default;

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{PropertyType} {DeclaringType}.{Name} ({PropertyAccess})";

    /// <inheritdoc />
    [Pure]
    public override MethodInfo? GetSetMethod(bool nonPublic) => null;

    /// <inheritdoc />
    [Pure]
    public override MethodInfo? GetGetMethod(bool nonPublic) => null;

    /// <inheritdoc />
    [Pure]
    public override bool IsDefined(Type attributeType, bool inherit) => GetCustomAttributes(attributeType, inherit).Any();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        => GetCustomAttributes(inherit)
        .Where(attr => attr.GetType() == attributeType)
        .ToArray();

    /// <inheritdoc />
    [Pure]
    public override MethodInfo[] GetAccessors(bool nonPublic) => Array.Empty<MethodInfo>();

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit) => Array.Empty<object>();

    /// <inheritdoc />
    [Pure]
    public override ParameterInfo[] GetIndexParameters() => Array.Empty<ParameterInfo>();

    /// <inheritdoc />
    [Pure]
    public override object? GetValue(object? obj, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        => throw new NotSupportedException();

    /// <inheritdoc />
    [Pure]
    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        => throw new NotSupportedException();

    /// <inheritdoc />
    public virtual void WriteTo(CSharpWriter writer)
    {
        foreach (var decoration in AttributeInfos) writer.Write(decoration);

        writer.Indent()
            .Write("public ").Write(PropertyType).Write(' ').Write(Name).Write(' ').Write(PropertyAccess.Code())
            .Line();
    }
}
