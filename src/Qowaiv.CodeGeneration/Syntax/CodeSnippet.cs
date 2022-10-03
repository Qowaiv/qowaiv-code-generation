using System.Text.RegularExpressions;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code snippet.</summary>
public sealed class CodeSnippet : Code
{
    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    public CodeSnippet(string? snippet)
        : this(snippet?.Split(new[] { "\r\n", "\n" }, default)) { }

    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    private CodeSnippet(string[]? lines)
        => Lines = lines ?? Array.Empty<string>();

    /// <summary>Gets the individual lines of the code snippet.</summary>
    public IReadOnlyList<string> Lines { get; }

    [Pure]
    public CodeSnippet Transform(Func<string, string> transformLine)
    {
        Guard.NotNull(transformLine, nameof(transformLine));
        return new(Lines.Select(transformLine).ToArray());
    }

    /// <summary>Transforms a code snippet.</summary>
    /// <param name="constants">
    /// The available constants.
    /// </param>
    /// <exception cref="InvalidOperationException"></exception>
    [Pure]
    public CodeSnippet Transform(IReadOnlyCollection<Constant> constants)
    {
        Guard.NotNull(constants, nameof(constants));

        var lines = new List<string>();

        var enabled = true;
        var mode = Mode.None;

        foreach (var line in Lines)
        {
            if (Patterns.If.Match(line) is { Success: true } @if)
            {
                if (mode != Mode.None) throw new InvalidOperationException();
                enabled = Enabled(@if, constants);
                mode = Mode.If;
            }
            if (Patterns.ElIf.Match(line) is { Success: true } elif)
            {
                if (mode != Mode.If || mode != Mode.ElIf) throw new InvalidOperationException();
                enabled = !enabled && Enabled(elif, constants);
                mode = Mode.ElIf;
            }
            else if (Patterns.Else.IsMatch(line))
            {
                if (mode != Mode.If || mode != Mode.ElIf) throw new InvalidOperationException();
                enabled = !enabled;
                mode = Mode.Else;
            }
            else if (Patterns.EndIf.IsMatch(line))
            {
                if (mode == Mode.None) throw new InvalidOperationException();
                enabled = true;
                mode = Mode.None;
            }
            else if (enabled)
            {
                lines.Add(line);
            }
        }
        return new(lines.ToArray());

        static bool Enabled(Match match, IReadOnlyCollection<Constant> constants)
        {
            var enabled = constants.Contains(match.Groups["constant"].Value);
            return match.Groups["negate"].Value is { }
                ? !enabled
                : enabled;
        }
    }

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        writer.Write(Lines.Select(Line), writer => writer.Line());
        static Action<CSharpWriter> Line(string line) => writer => writer.Write(line);
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();

    enum Mode
    {
        None = 0,
        If,
        ElIf,
        Else,
    }

    static class Patterns
    {
        public static readonly Regex If = new("^#if +(?<negate>!)?(?<constant>[A-Za-z0-9]+) *$", Options, Timeout);
        public static readonly Regex ElIf = new("^#elif +(?<negate>!)?(?<constant>[A-Za-z0-9]+) *$", Options, Timeout);
        public static readonly Regex Else = new("^#else *$", Options, Timeout);
        public static readonly Regex EndIf = new("^#endif *$", Options, Timeout);

        public static readonly RegexOptions Options = RegexOptions.Compiled | RegexOptions.CultureInvariant;
        public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(0.01);
    }
}
