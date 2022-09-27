namespace Qowaiv.OpenApi;

[DebuggerDisplay("Type = {Type}, Values = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public class Enumeration : Code, IReadOnlyCollection<EnumValue>
{
    public IReadOnlyCollection<EnumValue> Values { get; init; } = Array.Empty<EnumValue>();

    public int Count => Values.Count;

    public IEnumerator<EnumValue> GetEnumerator() => Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
