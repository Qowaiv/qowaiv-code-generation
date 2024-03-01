using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents the <see cref="PropertyInfo"/> of a <see cref="Type"/> property.</summary>
public partial class Property : PropertyInfo, Code
{
    protected readonly IReadOnlyCollection<Code> AttributeInfos;
    private readonly bool IsNullable;

    public Property(
        string name,
        Type propertyType,
        Type declaringClass,
        PropertyAccess access,
        IReadOnlyCollection<AttributeInfo>? attributes = null,
        XmlDocumentation? documentation = null,
        bool nullable = false,
        bool required = false)
    {
        Name = Guard.NotNullOrEmpty(name);
        PropertyType = Guard.NotNull(propertyType);
        DeclaringType = Guard.NotNull(declaringClass);
        PropertyAccess = Guard.DefinedEnum(access);
        AttributeInfos = attributes ?? Array.Empty<AttributeInfo>();
        Documentation = documentation ?? new XmlDocumentation();
        IsNullable = nullable;
        IsRequired = required;
    }

    public XmlDocumentation Documentation { get; }

    /// <inheritdoc />
    public override string Name { get; }

    /// <summary>True if the property is required.</summary>
    public bool IsRequired { get; }

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
    public override void SetValue(object? obj, object? value, BindingFlags invokeAttr, Binder? binder, object?[]? index, CultureInfo? culture)
        => throw new NotSupportedException();

    /// <inheritdoc />
    public virtual void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        writer.Write(Documentation);
        foreach (var decoration in AttributeInfos) writer.Write(decoration);

        writer.Indent()
            .Write("public ").Write(PropertyType).Write(IsNullable ? "?" : string.Empty).Write(' ').Write(Name).Write(' ').Write(PropertyAccess.Code());

        if (PropertyType.IsArray)
        {
            writer.Write(" = ").Write(typeof(Array)).Write(".Empty<").Write(PropertyType.GetElementType()!).Line(">();");
        }
        else { writer.Line(); }
    }
}
