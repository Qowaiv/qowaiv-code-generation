namespace Qowaiv.CodeGeneration;

/// <summary>Represents a namespace.</summary>
public readonly struct Namespace : IEquatable<Namespace>
{
    /// <summary>The empty/no namespace.</summary>
    public static readonly Namespace None;

    /// <summary>Creates a new instance of the <see cref="Namespace"/> struct.</summary>
    private Namespace(string name) => Name = Guard.NotNullOrEmpty(name, nameof(name));

    /// <summary>Gets the name of the namespace.</summary>
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string Name;

    /// <summary>Returns true if empty.</summary>
    public bool IsEmpty() => string.IsNullOrEmpty(Name);

    [Pure]
    public Namespace Child(string child)
        => IsEmpty()
        ? new(child)
        : new($"{Name}.{child}");

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => Name ?? "";

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? obj) => obj is Namespace other && Equals(other);

    /// <inheritdoc/>
    [Pure]
    public bool Equals(Namespace other) => Name == other.Name;

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => Name is null ? 0 : Name.GetHashCode();

    /// <summary>Returns true if both namespaces are the same.</summary>
    public static bool operator ==(Namespace left, Namespace right) => left.Equals(right);

    /// <summary>Returns false if both namespaces are the same.</summary>
    public static bool operator !=(Namespace left, Namespace right) => !(left == right);

    /// <summary>Implicitly casts a string to a namespace.</summary>
    public static implicit operator Namespace(string value) => new(value);
}
