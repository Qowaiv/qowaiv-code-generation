namespace Qowaiv.CodeGeneration;

/// <summary>Represents the name of a type and its namespace.</summary>
public sealed record TypeName
{
    /// <summary>Initializes a new instance of the <see cref="TypeName"/> class.</summary>
    public TypeName(Type type) : this(Guard.NotNull(type).Namespace!, type.Name) { }

    /// <summary>Initializes a new instance of the <see cref="TypeName"/> class.</summary>
    public TypeName(Namespace @namespace, string name)
    {
        Namespace = Guard.NotDefault(@namespace);
        Name = Guard.NotNullOrEmpty(name);
    }

    /// <summary>The namespace of the type.</summary>
    public Namespace Namespace { get; }

    /// <summary>The nae of the type.</summary>
    public string Name { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Namespace}.{Name}";
}
