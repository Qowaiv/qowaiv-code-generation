namespace Qowaiv.CodeGeneration.Syntax;

public sealed class NullableHeader : Code
{
    public static readonly NullableHeader Enable = new();

    private NullableHeader() { }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
        => Guard.NotNull(writer)
        .Line("#nullable enable")
        .Line();
}
