namespace Qowaiv.OpenApi;

[DebuggerDisplay("Type = {Type}, Properties = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public class CodeModel : Code, IReadOnlyCollection<CodeProperty>
{
    public CodeModel(CodeType type, OpenApiNamedSchema schema, IEnumerable<CodeProperty> properties)
    {
        Type = Guard.NotNull(type, nameof(type));
        Schema = Guard.NotNull(schema, nameof(schema));
        Properties = Guard.NotNull(properties, nameof(properties)).ToArray();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CodeProperty[] Properties;

    public CodeType Type { get; }

    public OpenApiNamedSchema Schema { get; }
    /// <inheritdoc />
    public int Count => Properties.Length;

    /// <inheritdoc />
    public IEnumerator<CodeProperty> GetEnumerator() => ((IEnumerable<CodeProperty>)Properties).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
