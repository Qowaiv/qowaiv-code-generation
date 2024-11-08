using Qowaiv.CodeGeneration.OpenApi.Collections;
using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

internal sealed record OpenApiTypeData : TypeData
{
    private readonly OpenApiResolveContext Context;

    /// <summary>Initializes a new instance of the <see cref="OpenApiTypeData"/> class.</summary>
    public OpenApiTypeData(OpenApiResolveContext context)
    {
        Attributes = Empty.HashSet<AttributeInfo>();
        Properties = Empty.Properties();
        Context = context;
    }

    internal required ResolveOpenApiSchema Schema { get; init; }

    /// <inheritdoc />
    public override Type? BaseType
    {
        get => GetBaseType();
        init => throw new NotSupportedException();
    }

    [Pure]
    private Type? GetBaseType() => Context.GetBaseType(this);

    /// <inheritdoc />
    public override bool IsAbstract
    {
        get => OneOfs.Count > 1;
        init => throw new NotSupportedException();
    }

    /// <inheritdoc />
    public override IReadOnlyCollection<Type> DerivedTypes
    {
        get => Context.GetDerivedTypes(this);
        init => throw new NotSupportedException();
    }

    /// <summary>The types of which all properties should be included.</summary>
    public IReadOnlyCollection<Type> AllOfs { get; init; } = Empty.Types();

    /// <summary>The types of which of one set of properties should be included.</summary>
    public IReadOnlyCollection<Type> OneOfs { get; init; } = Empty.Types();

    internal void AddAllOfs(IEnumerable<Type?> allOfs)
    {
        var props = (PropertyCollector)Properties;
        var types = (TypeCollector)AllOfs;
        types.AddRange(allOfs.OfType<Type>().Where(DifferentTypeName));
        props.AddRange(allOfs.OfType<Type>().SelectMany(t => t.GetProperties()));

        Context.UpdateHierachy(this);
    }

    internal void AddOneOfs(IEnumerable<Type?> oneOfs)
    {
        var types = (TypeCollector)OneOfs;
        types.AddRange(oneOfs.OfType<Type>().Where(DifferentTypeName));
        Context.UpdateHierachy(this);
    }

    internal void AddProperties(IEnumerable<PropertyInfo> properties)
    {
        var props = (PropertyCollector)Properties;
        props.AddRange(properties);
    }

    internal void AddAttributes(IEnumerable<AttributeInfo> attributes)
    {
        var hashset = (HashSet<AttributeInfo>)Attributes;
        hashset.AddRange(attributes);
    }

    [Pure]
    private bool DifferentTypeName(Type type)
        => type is not TypeBase tp
        || tp.TypeName != TypeName;
}
