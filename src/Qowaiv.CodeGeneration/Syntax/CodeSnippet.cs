using System.Text.RegularExpressions;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code snippet.</summary>
public sealed class CodeSnippet : Code
{
    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    public CodeSnippet(string? snippet)
        : this(snippet?.Split(new[] { "\r\n", "\n" }, default)) { }

    /// <summary>Creates a new instance of the <see cref="CodeSnippet"/> class.</summary>
    internal CodeSnippet(string[]? lines)
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

        var nr = 0;

        foreach (var line in Lines)
        {
            nr++;

            if (Matches(mode, nr, line, "#if", Patterns.If, m => m != Mode.None,  out var @if))
            {
                enabled = Enabled(@if, constants);
                mode = Mode.@if;
            }
            else if (Matches(mode, nr, line, "#elif", Patterns.ElIf, m => m != Mode.@if && m != Mode.elif, out var elif))
            {
                enabled = !enabled && Enabled(elif, constants);
                mode = Mode.elif;
            }
            else if (Matches(mode, nr, line, "#else", Patterns.Else, m => m != Mode.@if && m != Mode.elif, out _))
            {
                enabled = !enabled;
                mode = Mode.@else;
            }
            else if (Matches(mode, nr, line, "#endif", Patterns.EndIf, m => m == Mode.None, out _))
            {
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

        

        static bool Matches(Mode mode, int lineNr, string line, string startsWith, Regex pattern, Func<Mode, bool> unexpected, out Match match)
        {
            if (!line.StartsWith(startsWith, StringComparison.Ordinal))
            {
                match = null!;
                return false;
            }
            else if (pattern.Match(line) is { Success: true } m)
            {
                if (unexpected(mode)) throw ParseError.Line(lineNr, line, mode == Mode.None 
                    ? $"Unexpected {startsWith}."
                    : $"Unexpected {startsWith} after #{mode}");

                match = m;
                return match.Groups["exec"].Value is { };
            }
            else throw ParseError.Line(lineNr, line, "invalid pattern");
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

    private enum Mode
    {
        None = 0,
        @if,
        elif,
        @else,
    }

    static class Patterns
    {
        public static readonly RegexOptions Options = RegexOptions.Compiled | RegexOptions.CultureInvariant;
        public static readonly TimeSpan Timeout = TimeSpan.FromMilliseconds(1);

        public static readonly Regex If = new("^#if +(?<negate>!)?(?<constant>[A-Za-z0-9_]+) *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex ElIf = new("^#elif +(?<negate>!)?(?<constant>[A-Za-z0-9_]+) *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex Else = new("^#else *(?<exec>// *exec)? *$", Options, Timeout);
        public static readonly Regex EndIf = new("^#endif *(?<exec>// *exec)? *$", Options, Timeout);
    }
}
