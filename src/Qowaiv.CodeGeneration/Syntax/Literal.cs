using System.IO;

namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code literal.</summary>
public sealed class Literal : Code
{
    /// <summary>Initializes a new instance of the <see cref="Literal"/> class.</summary>
    public Literal(object? value) => Value = value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object? Value;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        Guard.NotNull(writer);

        if (Value is null) writer.Write("null");
        else if (Value is Type type) writer.Write("typeof(").Write(type).Write(')');
        else if (Value is bool logical) writer.Write(logical ? "true" : "false");
        else if (Value is int int32) writer.Write(int32.ToString(CultureInfo.InvariantCulture));
        else if (Value is double dbl) writer.Write(Double(dbl));
        else if (Value is decimal dec) writer.Write(dec.ToString(CultureInfo.InvariantCulture)).Write('m');
        else if (Value is string str) writer.Write(String(str));
        else throw new NotSupportedException($"Literals of type {Value.GetType()} are not supported");
    
    }

    [Pure]
    private static string Double(double dbl)
    {
        if (dbl == double.MinValue) return "double.MinValue";
        else if (dbl == double.MaxValue) return "double.MaxValue";
        else if (double.IsNaN(dbl)) return "double.NaN";
        else if (double.IsPositiveInfinity(dbl)) return "double.PositiveInfinity";
        else if (double.IsNegativeInfinity(dbl)) return "double.NegativeInfinity";
        else return dbl.ToString(CultureInfo.InvariantCulture);
    }

    [Pure]
    private static string String(string str)
    {
        return str.Contains('\\') || str.Contains('"')
            ? $@"@""{str}"""
            : $@"""{str}""";
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
