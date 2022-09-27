namespace Qowaiv.CodeGeneration;

public sealed class TypeName
{
    public TypeName(Namespace @namespace, string name)
    {
        Namespace = Guard.NotDefault(@namespace, nameof(@namespace));
        Name = Guard.NotNullOrEmpty(name, nameof(name));
    }

    public Namespace Namespace { get; }
    public string Name { get; }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => $"{Namespace}.{Name}";
}
