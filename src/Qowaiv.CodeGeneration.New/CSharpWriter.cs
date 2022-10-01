using Qowaiv.Diagnostics.Contracts;
using System.IO;

namespace Qowaiv.CodeGeneration;

public sealed class CSharpWriter
{
    /// <summary>UTF-8 BOM.</summary>
    public static readonly Encoding Encoding = new UTF8Encoding(true);

    private readonly CSharpWriterSettings Settings;
    private readonly TextWriter Writer;
    private int Indentation;

    /// <summary>Creates a new instance of the <see cref="CSharpWriter"/> class.</summary>
    public CSharpWriter(TextWriter writer) : this(writer, null) { }

    /// <summary>Creates a new instance of the <see cref="CSharpWriter"/> class.</summary>
    public CSharpWriter(TextWriter writer, CSharpWriterSettings? settings)
    {
        Writer = Guard.NotNull(writer, nameof(writer));
        Settings = settings ?? new();
    }

    /// <summary>Writes a character to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(char ch)
    {
        Writer.Write(ch);
        return this;
    }

    /// <summary>Writes code (represented as a string) to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(string? code)
    {
        Writer.Write(code);
        return this;
    }

    /// <summary>Writes the code to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Write(Code? code)
    {
        code?.WriteTo(this);
        return this;
    }

    [FluentSyntax]
    public CSharpWriter Write(IEnumerable<Action<CSharpWriter>> writes, Action<CSharpWriter> split)
    {
        Guard.NotNull(writes, nameof(writes));
        Guard.NotNull(split, nameof(split));

        var first = true;

        foreach(var write in writes)
        {
            if (!first) split(this);
            write(this);
            first = false;
        }

        return this;
    }

    [FluentSyntax]
    public CSharpWriter Write(Type type, bool attribute = false)
    {
        var name = type.ToCSharpString(withNamespace: !Settings.Globals.Contains(type.Namespace!));
        return attribute && name.EndsWith("Attribute")
            ? Write(name[..^9])
            : Write(name);
    }

    /// <summary>Writes a literal to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Literal(object? value) => Write(new Syntax.Literal(value));

    /// <summary>Indents the current line of the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Indent()
    {
        foreach (var code in Enumerable.Repeat(Settings.Indentation, Indentation))
        {
            Write(code);
        }
        return this;
    }

    /// <summary>Writes the line including an ending to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Line(string? line = null) => Write(line).Write(Settings.NewLine);

    /// <summary>Writes the character including an ending to the code file.</summary>
    [FluentSyntax]
    public CSharpWriter Line(char ch) => Write(ch).Write(Settings.NewLine);

    /// <summary>Writes a code block (`{ ... }`).</summary>
    [Pure]
    public IDisposable CodeBlock(string markers = "{}")
    {
        Line(markers[0]);
        Indentation++;
        return new ScopedCodeBlock(this, markers[1]);
    }

    /// <summary>
    /// Clears all buffers for the current writer and causes any buffered data
    /// to be written to the code file.
    /// </summary>
    public void Flush() => Writer.Flush();

    private sealed record ScopedCodeBlock(CSharpWriter Writer, char Close) : IDisposable
    {
        public void Dispose()
        {
            Writer.Indentation--;
            Writer.Indent().Line(Close);
        }
    }
}
