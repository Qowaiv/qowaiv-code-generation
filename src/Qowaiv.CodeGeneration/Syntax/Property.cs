using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents the <see cref="PropertyInfo"/> of a <see cref="Type"/> property.</summary>
public partial class Property : PropertyInfo, Code
{
    /// <summary>The type data.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    protected PropertyData Data { get; }

    /// <summary>Initializes a new instance of the <see cref="Property"/> class.</summary>
    public Property(PropertyData data) => Data = Guard.NotNull(data);

    /// <summary>Gets the attributes for the property.</summary>
    public IReadOnlyCollection<AttributeInfo> AttributeInfos => Data.Attributes ?? [];

    /// <summary>Gets the documentation for the properties.</summary>
    public XmlDocumentation Documentation => Data.Documentation ?? new();

    /// <inheritdoc />
    public override string Name => Data.Name.ToString(Data.NameConvention.ForProperty(DeclaringType));

    /// <summary>True if the property is required.</summary>
    public bool IsRequired => Data.IsRequired;

    /// <summary>True if the property is nullable.</summary>
    public bool IsNullable => Data.IsNullable;

    /// <inheritdoc />
    public override Type PropertyType => Data.PropertyType;

    /// <inheritdoc />
    public override Type DeclaringType => Data.DeclaringType;

    /// <summary>Gets the (get/set/init) access to the property.</summary>
    public PropertyAccess PropertyAccess => Data.PropertyAccess;

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
    public override MethodInfo[] GetAccessors(bool nonPublic) => [];

    /// <inheritdoc />
    [Pure]
    public override object[] GetCustomAttributes(bool inherit) => [];

    /// <inheritdoc />
    [Pure]
    public override ParameterInfo[] GetIndexParameters() => [];

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
            .Write("public ")
            .Write(writer.Settings.UseRequiredModifier && IsRequired ? "required " : string.Empty)
            .Write(PropertyType)
            .Write(IsNullable ? "?" : string.Empty).Write(' ')
            .Write(Name).Write(' ')
            .Write(PropertyAccess.Code());

        if (PropertyType.IsArray)
        {
            writer.Write(" = ").Write(typeof(Array)).Write(".Empty<").Write(PropertyType.GetElementType()!).Line(">();");
        }
        else { writer.Line(); }
    }
}
