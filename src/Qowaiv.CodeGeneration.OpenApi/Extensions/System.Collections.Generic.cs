using Qowaiv.CodeGeneration;

namespace System.Collections.Generic;

/// <summary>Extensions on <see cref="ISet{T}"/>.</summary>
internal static class QowaivOpenApiCollectionsExtensions
{
    /// <summary>Exposes <see cref="IList{T}"/> as <see cref="IReadOnlyList{T}"/>.</summary>
    /// <remarks>
    /// As <see cref="IList{T}"/> does not inherit from <see cref="IReadOnlyList{T}"/>
    /// this wrapper is needed.
    /// </remarks>
    [Pure]
    public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
        => new ReadOnlyListWrapper<T>(Guard.NotNull(list));

    /// <summary>Exposes <see cref="ISet{T}"/> as <see cref="IReadOnlySet{T}"/>.</summary>
    /// <remarks>
    /// As <see cref="ISet{T}"/> does not inherit from <see cref="IReadOnlySet{T}"/>
    /// this wrapper is needed.
    /// </remarks>
    [Pure]
    public static IReadOnlySet<T> AsReadOnlySet<T>(this ISet<T> set)
        => new ReadOnlySetWrapper<T>(Guard.NotNull(set));

    private readonly struct ReadOnlyListWrapper<T>(IList<T> list) : IReadOnlyList<T>
    {
        private readonly IList<T> List = list;

        /// <inheritdoc />
        public T this[int index] => List[index];

        /// <inheritdoc />
        public int Count => List.Count;

        /// <inheritdoc />
        [Pure]
        public IEnumerator<T> GetEnumerator() => List.GetEnumerator();

        /// <inheritdoc />
        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

    private readonly struct ReadOnlySetWrapper<T>(ISet<T> set) : IReadOnlySet<T>
    {
        private readonly ISet<T> Set = set;

        /// <inheritdoc />
        public int Count => Set.Count;

        /// <inheritdoc />
        [Pure]
        public bool Contains(T item) => Set.Contains(item);

        /// <inheritdoc />
        [Pure]
        public bool IsProperSubsetOf(IEnumerable<T> other) => Set.IsProperSubsetOf(other);

        /// <inheritdoc />
        [Pure]
        public bool IsProperSupersetOf(IEnumerable<T> other) => Set.IsProperSupersetOf(other);

        /// <inheritdoc />
        [Pure]
        public bool IsSubsetOf(IEnumerable<T> other) => Set.IsSubsetOf(other);

        /// <inheritdoc />
        [Pure]
        public bool IsSupersetOf(IEnumerable<T> other) => Set.IsSupersetOf(other);

        /// <inheritdoc />
        [Pure]
        public bool Overlaps(IEnumerable<T> other) => Set.Overlaps(other);

        /// <inheritdoc />
        [Pure]
        public bool SetEquals(IEnumerable<T> other) => Set.SetEquals(other);

        /// <inheritdoc />
        [Pure]
        public IEnumerator<T> GetEnumerator() => Set.GetEnumerator();

        /// <inheritdoc />
        [Pure]
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
