using System.IO;

namespace Qowaiv.CodeGeneration;

/// <summary>Represents a .NET namespace.</summary>
public readonly struct Namespace : IEquatable<Namespace>
{
    /// <summary>Initializes a new instance of the <see cref="Namespace"/> struct.</summary>
    public Namespace(string name) => Name = Guard.NotNullOrEmpty(name, nameof(name));

    /// <summary>Gets the name of the namespace.</summary>
    public string Name { get; }

    /// <summary>Get the parent namespace of the namespace.</summary>
    public Namespace Parent
    {
        get
        {
            var index = Name.LastIndexOf('.');
            return index == -1 ? default : new(Name[..index]);
        }
    }

    /// <summary>Returns true if empty.</summary>
    [Pure]
    public bool IsEmpty() => string.IsNullOrEmpty(Name);

    [Pure]
    public Namespace Child(string child)
        => IsEmpty()
        ? new(child)
        : new($"{Name}.{child}");

    /// <summary>Gets the code declaration.</summary>
    [Pure]
    public Code Declaration() => new Syntax.NamespaceDeclaration(this);

    /// <inheritdoc/>
    [Pure]
    public override string ToString() => Name ?? string.Empty;

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

    [Pure]
    public static IEnumerable<Namespace> Globals(FileInfo file)
        => Globals(Guard.NotNull(file, nameof(file)).OpenRead());

    [Pure]
    public static IEnumerable<Namespace> Globals(Stream stream)
    {
        Guard.NotNull(stream, nameof(Stream));
        using var reader = new StreamReader(stream);
        while (reader.ReadLine() is { } line)
        {
            if (line.StartsWith("global using ") && line[^1] == ';')
            {
                yield return new Namespace(line[13..^1]);
            }
        }
    }
}
