using System.Runtime.Serialization;

namespace Qowaiv.CodeGeneration.Syntax;

public sealed class ParseError : InvalidOperationException
{
    /// <summary>Creates new instance of the <see cref="ParseError"/> exception.</summary>
    public ParseError() { }

    /// <summary>Creates new instance of the <see cref="ParseError"/> exception.</summary>
    public ParseError(string? message) : base(message) { }

    /// <summary>Creates new instance of the <see cref="ParseError"/> exception.</summary>
    public ParseError(SerializationInfo info, StreamingContext context) : base(info, context) { }

    /// <summary>Creates new instance of the <see cref="ParseError"/> exception.</summary>
    public ParseError(string? message, Exception? innerException) : base(message, innerException) { }

    /// <summary>Creates a line based parse error.</summary>
    [Pure]
    public static ParseError Line(int lineNr, string line, string message) => new($"[{lineNr}] {line}: {message}");
}
