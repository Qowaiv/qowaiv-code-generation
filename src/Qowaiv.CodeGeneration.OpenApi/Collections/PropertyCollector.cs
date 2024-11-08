using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi.Collections;

internal sealed class PropertyCollector : HashSet<PropertyInfo>
{
    private static readonly EqualityComparer PropComparer = new();

    public PropertyCollector() : base(PropComparer) { }

    public void AddRange(IEnumerable<PropertyInfo> properties) => QowaivOpenApiCollectionsExtensions.AddRange(this, properties);

    private sealed class EqualityComparer : IEqualityComparer<PropertyInfo>
    {
        [Pure]
        public bool Equals(PropertyInfo? x, PropertyInfo? y)
            => x?.Name == y?.Name
            && x?.PropertyType.ToCSharpString() == y?.PropertyType.ToCSharpString();

        [Pure]
        public int GetHashCode(PropertyInfo obj) => HashCode.Combine(obj.Name, obj.PropertyType.ToCSharpString());
    }
}
