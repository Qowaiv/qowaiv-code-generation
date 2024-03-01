using System.IO;

namespace Qowaiv.CodeGeneration.IO;

public sealed class CodeFileWriterSettings
{
    public DirectoryInfo RootDirectory { get; init; } = new(".");

    public Namespace RootNamespace { get; init; }

    public string? FileNameSuffix { get; init; } = ".generated";

    public string? DeletePattern { get; init; } = "*.generated.cs";

    public IReadOnlyCollection<Code> Headers { get; init; } = Array.Empty<Code>();
}
