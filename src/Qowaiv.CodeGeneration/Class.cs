using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents the <see cref="Type"/> of a class.</summary>
public sealed class Class : TypeInfo
{
    /// <summary>Creates a new instance of the <see cref="Class"/> class.</summary>
    public Class(TypeName nameType, IReadOnlyCollection<Property>? properties = null, bool isArray = false, Type? baseType = null)
        : base(nameType, isArray, baseType)
    {
        Properties = properties ?? Array.Empty<Property>();
    }

    /// <summary>Gets the properties of this type.</summary>
    public IReadOnlyCollection<Property> Properties { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Namespace}.{Name}{ArraySuffix()}, Properties: {Properties.Count}";

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType()
        => IsArray
        ? new Class(new(NS, Name), Properties, isArray: false, BaseType)
        : null;

    /// <inheritdoc />
    [Pure]
    public override Type MakeArrayType()
         => IsArray
        ? this
        : new Class(new(NS, Name), Properties, isArray: true, BaseType);

    /// <inheritdoc />
    [Pure]
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        => bindingAttr.HasFlag(BindingFlags.Instance | BindingFlags.Public)
        ? Properties.ToArray()
        : Array.Empty<PropertyInfo>();
}
