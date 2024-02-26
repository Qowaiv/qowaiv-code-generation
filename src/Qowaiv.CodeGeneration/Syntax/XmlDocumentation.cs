using System.Xml.Linq;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>The XML documentation.</summary>
public sealed class XmlDocumentation : Code
{
    /// <summary>The summary.</summary>
    public string? Summary { get; init; }

    /// <summary>The remarks.</summary>
    public string? Remarks { get; init; }

    /// <summary>The collection with parameters.</summary>
    public IReadOnlyDictionary<string, string> Params { get; init; } = new Dictionary<string, string>();

    private static readonly string[] NewLineChars = ["\r\n", "\n"];

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

    private static void WriteText(CSharpWriter writer, string? text, string tag)
    {
        if (text is { Length: > 0 })
        {
            var lines = Trim(text.Split(NewLineChars, StringSplitOptions.None));

            if (lines.Length == 1)
            {
                writer.Indent().Line($"/// <{tag}>{Escape(lines[0]).TrimEnd()}</{tag}>");
            }
            else
            {
                writer.Indent().Line($"/// <{tag}>");
                foreach (var line in lines.Select(Escape))
                {
                    writer.Indent().Line($"/// {line.TrimEnd()}");
                }
                writer.Indent().Line($"/// </{tag}>");
            }
        }
    }

    [Pure]
    private static string[] Trim(string[] lines)
        => lines
            .SkipWhile(l => l.Trim().Length == 0)
            .TakeWhile(l => l.Trim().Length > 0)
            .ToArray();

    [Pure]
    private static string Escape(string s) => new XText(s).ToString();
}
