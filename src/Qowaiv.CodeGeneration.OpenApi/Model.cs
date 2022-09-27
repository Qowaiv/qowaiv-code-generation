namespace Qowaiv.OpenApi;


/// <summary>Collects TypeScript code files to generate.</summary>
/// <remarks>
/// This class is immutable. So every addition returns a new instance.
/// </remarks>
[DebuggerDisplay("Type = {Type}, Properties = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public class Model : Code, IReadOnlyCollection<Property>
{
    public IReadOnlyCollection<Property> Properties { get; init; } = Array.Empty<Property>();

    public int Count => Properties.Count;

    public IEnumerator<Property> GetEnumerator()=> Properties.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
