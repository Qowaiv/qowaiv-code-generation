namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a code literal.</summary>
public sealed class Literal : Code
{
    /// <summary>Creates a new instance of the <see cref="Literal"/> class.</summary>
    public Literal(object? value) => Value = value;

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly object? Value;

    /// <inheritdoc />
    public void WriteTo(CSharpWriter writer)
    {
        if (Value is null) writer.Write("null");
        else if (Value is Type type) writer.Write("typeof(").Write(type).Write(')');
        else if (Value is bool logical) writer.Write(logical ? "true" : "false");
        else if (Value is double db) writer.Write(db.ToString(CultureInfo.InvariantCulture)).Write('d');
        else if (Value is decimal dec) writer.Write(dec.ToString(CultureInfo.InvariantCulture)).Write('m');
        else if (Value is string str) writer.Write('"').Write(str.Replace(@"""", @"\""")).Write('"');
        else throw new NotSupportedException();
    }

    /// <inheritdoc />
    [Pure]
    public override string ToString() => this.Stringify();
}
