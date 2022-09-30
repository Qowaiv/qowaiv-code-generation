namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code snippet.</summary>
public class CodeSnippet : Code
{
    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly string[] Lines;

    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    public CodeSnippet(string? snippet)
        : this(snippet?.Split(new[] { "\r\n", "\n" }, default)) { }

    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    protected CodeSnippet(string[]? lines)
        => Lines = lines ?? Array.Empty<string>();

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        writer.Write(Lines.Select(Line), writer => writer.Line());
        static Action<CSharpWriter> Line(string line) => writer => writer.Write(line);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
