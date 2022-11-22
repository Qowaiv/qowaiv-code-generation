using System.Reflection;

namespace Qowaiv.CodeGeneration.Syntax;

[DebuggerDisplay("Name = {Name} FullName = {FullName} ArrayRank = {ArrayRank}")]
internal sealed class ArrayType : ObjectBase
{
    private readonly ObjectBase ElementType;
    private readonly int ArrayRank;

    public ArrayType(ObjectBase type,
        int arrayRank,
        IReadOnlyCollection<AttributeInfo>? attributes,
        IReadOnlyCollection<ConstructorInfo>? constructors,
        IReadOnlyCollection<EventInfo>? events,
        IReadOnlyCollection<FieldInfo>? fields,
        IReadOnlyCollection<MethodInfo>? methods,
        IReadOnlyCollection<PropertyInfo>? properties,
        IReadOnlyCollection<Type>? interfaces)
        :base(type.TypeName,
            type.BaseType,
            attributes,
            constructors,
            events,
            fields,
            methods,
            properties,
            interfaces)
    {
        ElementType = type;
        ArrayRank = arrayRank;
    }

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => ElementType;

    /// <inheritdoc />
    [Pure]
    protected override bool IsArrayImpl() => true;

    /// <inheritdoc />
    [Pure]
    public override int GetArrayRank()=> ArrayRank;
}
