namespace Qowaiv.CodeGeneration;

[DebuggerDisplay("Name = {Name} FullName = {FullName} ArrayRank = {ArrayRank}")]
internal sealed class ArrayType(TypeBase type, int arrayRank)
    : TypeBase(new TypeInfo
    {
        TypeName = Guard.NotNull(type).TypeName,
        BaseType = typeof(Array),
    })
{
    private readonly TypeBase ElementType = type;
    private readonly int ArrayRank = arrayRank;

    /// <inheritdoc />
    [Pure]
    public override Type? GetElementType() => ElementType;

    /// <inheritdoc />
    [Pure]
    protected override bool IsArrayImpl() => true;

    /// <inheritdoc />
    [Pure]
    public override int GetArrayRank() => ArrayRank;
}
