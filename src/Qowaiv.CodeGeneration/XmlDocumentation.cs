namespace Qowaiv.CodeGeneration;

public sealed class XmlDocumentation : Code
{
    public string? Summary { get; init; }
    public string? Remarks { get; init; }
    public IReadOnlyDictionary<string, string> Params { get; init; } = new Dictionary<string, string>();

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer, nameof(writer));

        if (Summary is { Length: > 0 })
        {
            writer.Indent().Line($"/// <summary>{Summary}</summary>");
        }
        foreach (var param in Params ?? new Dictionary<string, string>())
        {
            writer.Indent().Line($"/// <param name=\"{param.Key}\">{param.Value}.</param>");
        }
        if (Remarks is { Length: > 0 })
        {
            writer.Indent().Line($"/// <remarks>{Remarks}</remarks>");
        }
    }
}
