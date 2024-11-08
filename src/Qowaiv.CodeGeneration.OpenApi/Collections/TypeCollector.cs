using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi.Collections;

internal sealed class TypeCollector : HashSet<Type>
{
    private static readonly EqualityComparer PropComparer = new();

    public TypeCollector() : base(PropComparer) { }

    [FluentSyntax]
    public TypeCollector AddRange(IEnumerable<Type> types)
    {
        QowaivOpenApiCollectionsExtensions.AddRange(this, types);

        return this;
    }

    private sealed class EqualityComparer : IEqualityComparer<Type>
    {
        [Pure]
        public bool Equals(Type? x, Type? y)
            => x?.ToCSharpString() == y?.ToCSharpString();

        [Pure]
        public int GetHashCode(Type obj) => obj.ToCSharpString().GetHashCode();
    }
}
