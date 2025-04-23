namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>
/// Represents a file header to indicate that #pragma warnings have been disabled.
/// </summary>
public sealed class DisableWarningsHeader : Code
{
    internal DisableWarningsHeader() { }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer) => Guard.NotNull(writer)
        .Line("#pragma warning disable")
        .Line();

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
