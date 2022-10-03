namespace Qowaiv.CodeGeneration.Syntax;

public sealed class NamespaceDeclaration : Code
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly Namespace Namespace;

    public NamespaceDeclaration(Namespace @namespace) => Namespace = Guard.NotDefault(@namespace, nameof(@namespace));

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
        => Guard.NotNull(writer, nameof(writer)).Write($"namespace {Namespace};");

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
