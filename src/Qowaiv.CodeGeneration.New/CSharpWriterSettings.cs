namespace Qowaiv.CodeGeneration;

public record CSharpWriterSettings
{
    public IReadOnlyCollection<Namespace> Globals { get; init; } = Array.Empty<Namespace>();

    public string NewLine { get; init; } = "\r\n";
    public string Indentation { get; init; } = "    ";
    public Encoding Encoding { get; init; } = new UTF8Encoding(true);
}
