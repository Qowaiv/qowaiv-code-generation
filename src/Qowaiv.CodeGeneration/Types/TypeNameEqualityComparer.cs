namespace Qowaiv.CodeGeneration;

public sealed class TypeNameEqualityComparer : IEqualityComparer<Type>
{
    /// <inheritdoc />
    [Pure]
    public bool Equals(Type? x, Type? y)
        => x is { } && y is { }
        ? x.ToCSharpString(true) == y.ToCSharpString(true)
        : ReferenceEquals(x, y);

    /// <inheritdoc />
    [Pure]
    public int GetHashCode([DisallowNull] Type obj) => obj.ToCSharpString(true).GetHashCode();
}
