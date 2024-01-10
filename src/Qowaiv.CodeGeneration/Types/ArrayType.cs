namespace Qowaiv.CodeGeneration;

[DebuggerDisplay("Name = {Name} FullName = {FullName} ArrayRank = {ArrayRank}")]
internal sealed class ArrayType : TypeBase
{
    private readonly TypeBase ElementType;
    private readonly int ArrayRank;

    public ArrayType(TypeBase type, int arrayRank)
        :base(new TypeInfo
        {
            TypeName = Guard.NotNull(type).TypeName,
            BaseType = typeof(Array),
        })
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
