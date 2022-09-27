using System.Reflection;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents the <see cref="Type"/> of a enum.</summary>
public sealed class Enumeration : TypeInfo
{
    /// <summary>Creates a new instance of the <see cref="Enumeration"/> class.</summary>
    public Enumeration(
        TypeName nameType,
        IReadOnlyCollection<EnumerationField>? fields = null,
        bool isArray = false)
        : base(nameType, isArray, typeof(Enum))
    {
        Fields = fields ?? Array.Empty<EnumerationField>();
    }

    /// <summary>Gets the properties of this type.</summary>
    public IReadOnlyCollection<EnumerationField> Fields { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Namespace}.{Name}{ArraySuffix()}, Fields: {Fields.Count}";

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType()
        => IsArray
        ? new Enumeration(new(NS, Name), Fields, isArray: false)
        : null;

    /// <inheritdoc />
    [Pure]
    public override Type MakeArrayType()
         => IsArray
        ? this
        : new Enumeration(new(NS, Name), Fields, isArray: true);

    /// <inheritdoc />
    [Pure]
    public override PropertyInfo[] GetProperties(BindingFlags bindingAttr)
        => Array.Empty<PropertyInfo>();
}
