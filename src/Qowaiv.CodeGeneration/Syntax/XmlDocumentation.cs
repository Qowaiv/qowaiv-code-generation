namespace Qowaiv.CodeGeneration.Syntax;

public sealed class XmlDocumentation : Code
{
    public string? Summary { get; init; }
    public string? Remarks { get; init; }
    public IReadOnlyDictionary<string, string> Params { get; init; } = new Dictionary<string, string>();

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        WriteText(writer, Summary, "summary");
        
        foreach (var param in Params ?? new Dictionary<string, string>())
        {
            writer.Indent().Line($"/// <param name=\"{param.Key}\">{param.Value}.</param>");
        }

        WriteText(writer, Remarks, "remarks");
    }

    static void WriteText(CSharpWriter writer, string? text, string tag)
    {
        if(text is { Length: > 0})
        {
            var lines = text.Split(new [] { "\r\n", "\n" }, StringSplitOptions.None);

            if (lines.Length == 1)
            {
                writer.Indent().Line($"/// <{tag}>{lines[0].TrimEnd()}</{tag}>");
            }
            else
            {
                writer.Indent().Line($"/// <{tag}>");
                foreach(var line in lines)
                {
                    writer.Indent().Line($"/// {line.TrimEnd()}");
                }
                writer.Indent().Line($"/// </{tag}>");
            }
        }
    }
}
