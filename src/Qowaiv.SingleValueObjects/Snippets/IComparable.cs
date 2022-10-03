public partial struct @TSvo : IComparable, IComparable<@TSvo>
{
    /// <inheritdoc />
    [Pure]
    public int CompareTo(object? obj)
    {
        if (obj is null) { return 1; }
        else if (obj is @TSvo other) { return CompareTo(other); }
        else { throw new ArgumentException($"Argument must be {GetType().Name}.", nameof(obj)); }
    }
#if !NotEqualsSvo
    /// <inheritdoc />
    [Pure]
#nullable disable
    public int CompareTo(@TSvo other) => Comparer<@type>.Default.Compare(m_Value, other.m_Value);
#nullable enable
#endif
#if !NotComparisonOperators
    /// <summary>Returns true if the left operator is less then the right operator, otherwise false.</summary>
    public static bool operator <(@TSvo l, @TSvo r) => l.CompareTo(r) < 0;

    /// <summary>Returns true if the left operator is greater then the right operator, otherwise false.</summary>
    public static bool operator >(@TSvo l, @TSvo r) => l.CompareTo(r) > 0;

    /// <summary>Returns true if the left operator is less then or equal the right operator, otherwise false.</summary>
    public static bool operator <=(@TSvo l, @TSvo r) => l.CompareTo(r) <= 0;

    /// <summary>Returns true if the left operator is greater then or equal the right operator, otherwise false.</summary>
    public static bool operator >=(@TSvo l, @TSvo r) => l.CompareTo(r) >= 0;
#endif
}
