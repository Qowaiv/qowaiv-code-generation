namespace Qowaiv.CodeGeneration;

public record CSharpWriterSettings
{
    /// <summary>Available (global) namespace usings.</summary>
    public IReadOnlyCollection<Namespace> GlobalUsings { get; init; } = Array.Empty<Namespace>();

    /// <summary>New line character(s).</summary>
    public string NewLine { get; init; } = "\r\n";

    /// <summary>Indentation character(s).</summary>
    public string Indentation { get; init; } = "    ";

    /// <summary>File encoding.</summary>
    public Encoding Encoding { get; init; } = new UTF8Encoding(true);
}
