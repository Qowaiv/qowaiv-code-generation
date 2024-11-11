using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Diagnostics.Contracts;

namespace Qowaiv.CodeGeneration.OpenApi;

public sealed class OpenApiResolveContext
{
    private readonly Dictionary<OpenApiPath, Entry> ByPath = [];
    private readonly Dictionary<ReferenceId, Entry> ById = [];
    private readonly Dictionary<TypeBase, Hierarchy> Hierarchies = [];

    [Pure]
    public Type? Get(ResolveOpenApiSchema schema) => GetEntry(schema)?.Type;

    [Pure]
    internal Entry? GetEntry(ResolveOpenApiSchema schema)
    {
        _ = ById.TryGetValue(schema.ReferenceId, out var entry) ||
            ByPath.TryGetValue(schema.Path, out entry);

        return entry;
    }

    [FluentSyntax]
    internal Type? Add(ResolveOpenApiSchema schema, Type? type, OpenApiTypeData? data = null)
    {
        if (type is null) return null;

        var entry = new Entry(schema.ReferenceId, schema.Path, type, data);

        if (schema.ReferenceId.HasValue)
        {
            ById.TryAdd(schema.ReferenceId, entry);
        }
        ByPath.TryAdd(schema.Path, entry);

        return type;
    }

    [Pure]
    internal IReadOnlyCollection<Code> ResolvedCode(Func<Class, ResolveOpenApiSchema, IEnumerable<AttributeInfo>> decorate)
    {
        var collection = new List<Code>();

        foreach (var entry in GetEntries().Where(e => e.Type is Code))
        {
            var code = (Code)entry.Type!;
            collection.Add(code);

            if (entry.Type is Class @class && entry.Data is { } data)
            {
                data.AddAttributes(decorate(@class, data.Schema));
            }
        }

        return collection;
    }

    [Pure]
    internal Type? GetBaseType(OpenApiTypeData data)
    {
        var hierarchy = GetHierachy(data.TypeName);
        return hierarchy.Bases is { Count: 1 } single
            ? single.First()
            : null;
    }

    [Pure]
    internal IReadOnlyCollection<Type> GetDerivedTypes(OpenApiTypeData data) => GetHierachy(data.TypeName).Derived;

    internal void UpdateHierachy(OpenApiTypeData data)
    {
        var type = GetType(data.TypeName);

        var hierarchy = GetHierachy(type);
        hierarchy.Bases.AddRange(data.AllOfs);
        hierarchy.Derived.AddRange(data.OneOfs);

        foreach (var @base in data.AllOfs.OfType<TypeBase>())
        {
            GetHierachy(@base).Derived.Add(type);
        }
        foreach (var derived in data.OneOfs.OfType<TypeBase>())
        {
            GetHierachy(derived).Bases.Add(type);
        }
    }

    [Pure]
    private TypeBase GetType(TypeName name)
        => (TypeBase)GetEntries().First(e => e.Type is TypeBase type && type.TypeName == name).Type;

    [Pure]
    private Hierarchy GetHierachy(TypeName name) => GetHierachy(GetType(name));

    [Pure]
    private Hierarchy GetHierachy(TypeBase type)
    {
        if (!Hierarchies.TryGetValue(type, out var hierarchy))
        {
            hierarchy = new(type.TypeName, [], []);
            Hierarchies[type] = hierarchy;
        }
        return hierarchy;
    }

    [Pure]
    private IEnumerable<Entry> GetEntries() => ByPath.Values.Distinct();

    internal sealed record Entry(ReferenceId ReferenceId, OpenApiPath Path, Type Type, OpenApiTypeData? Data)
    {
        [Pure]
        public override string ToString() => $"{Type.FullName}";
    }

    private sealed record Hierarchy(TypeName Name, HashSet<Type> Bases, HashSet<Type> Derived)
    {
        [Pure]
        public override string ToString()
            => $"{Name}, "
            + $"Bases: [{string.Join(", ", Bases.Select(b => b.Name))}], "
            + $"Derived: [{string.Join(", ", Derived.Select(b => b.Name))}]";
    }
}
