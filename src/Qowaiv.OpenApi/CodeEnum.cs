namespace Qowaiv.OpenApi;

[DebuggerDisplay("Type = {Type}, Values = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public class CodeEnum : Code, IReadOnlyCollection<CodeNameValue>
{
    public CodeEnum(CodeType type, OpenApiNamedSchema schema, IEnumerable<CodeNameValue> nameValues)
    {
        Type = Guard.NotNull(type, nameof(type));
        Schema = Guard.NotNull(schema, nameof(schema));
        Values = Guard.NotNull(nameValues, nameof(nameValues)).ToArray();
    }

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly CodeNameValue[] Values;

    public CodeType Type { get; }
    public OpenApiNamedSchema Schema { get; }

    /// <inheritdoc />
    public int Count => Values.Length;

    /// <inheritdoc />
    public IEnumerator<CodeNameValue> GetEnumerator() => ((IEnumerable<CodeNameValue>)Values).GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
