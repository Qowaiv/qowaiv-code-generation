namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a file scoped namespace declaration.</summary>
public sealed class NamespaceDeclaration(Namespace @namespace) : Code
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Namespace Namespace = Guard.NotDefault(@namespace);

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
        => Guard.NotNull(writer).Line($"namespace {Namespace};");

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
