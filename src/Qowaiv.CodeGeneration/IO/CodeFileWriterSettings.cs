using System.IO;

namespace Qowaiv.CodeGeneration.IO;

/// <summary>Settings to determine where to write code files to disk.</summary>
public sealed class CodeFileWriterSettings
{
    /// <summary>The root directory of the files to write.</summary>
    public DirectoryInfo RootDirectory { get; init; } = new(".");

    /// <summary>The root namespace that is stripped before determining the subdirectory.</summary>
    public Namespace RootNamespace { get; init; }

    /// <summary>The file name suffix added to the files generated.</summary>
    public string? FileNameSuffix { get; init; } = ".generated";

    /// <summary>The pattern used to delete existing files.</summary>
    public string? DeletePattern { get; init; } = "*.generated.cs";

    /// <summary>A collection of code to add to the header of the files.</summary>
    public IReadOnlyCollection<Code> Headers { get; init; } = [];
}
