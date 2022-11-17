namespace Qowaiv.CodeGeneration.Syntax;

public class Documentation : Code
{
    public Documentation(string? summary, string? remarks = null)
    {
        Summary = summary;
        Remarks = remarks;
    }

    public string? Summary { get; }
    public string? Remarks { get; }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));

        if (Summary is { Length: > 0 })
        {
            writer.Indent().Line($"<summary>{Summary}</summary>");
        }
        if (Remarks is { Length: > 0 })
        {
            writer.Indent().Line($"<summary>{Remarks}</summary>");
        }
    }
}
