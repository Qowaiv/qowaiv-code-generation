﻿namespace Qowaiv.CodeGeneration;

/// <summary>Represents a constant.</summary>
public readonly struct Constant : IEquatable<Constant>
{
    public readonly string? Name;

    public Constant(string name) => Name = Guard.NotNullOrEmpty(name, nameof(name));

    /// <inheritdoc />
    [Pure]
    public override string ToString() => Name ?? string.Empty;

    /// <inheritdoc/>
    [Pure]
    public override bool Equals(object? obj) => obj is Namespace other && Equals(other);

    /// <inheritdoc/>
    [Pure]
    public bool Equals(Constant other) => Name == other.Name;

    /// <inheritdoc/>
    [Pure]
    public override int GetHashCode() => Name is null ? 0 : Name.GetHashCode();

    /// <summary>Returns true if both namespaces are the same.</summary>
    public static bool operator ==(Constant left, Constant right) => left.Equals(right);

    /// <summary>Returns false if both namespaces are the same.</summary>
    public static bool operator !=(Constant left, Constant right) => !(left == right);

    /// <summary>Implicitly casts a string to a namespace.</summary>
    public static implicit operator Constant(string value) => new(value);
}
