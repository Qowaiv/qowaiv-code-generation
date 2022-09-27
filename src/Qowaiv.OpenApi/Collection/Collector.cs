using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using System.IO;

namespace Qowaiv.OpenApi.Collection;

/// <summary>Collects code files to generate.</summary>
/// <remarks>
/// This class is immutable. So every addition returns a new instance.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public sealed class Collector : IReadOnlyCollection<Code>
{
    private readonly Dictionary<CodeType, Code> Models = new();
    private readonly EnumValueNaming EnumValueNaming;
    private readonly PropertyNaming PropertyNaming;

    private Collector(
        TypeResolver resolver,
        EnumValueNaming enumNaming,
        PropertyNaming propertyNaming,
        
        IEnumerable<Code> models)
    {
        Resolver = resolver;
        EnumValueNaming = enumNaming;
        PropertyNaming = propertyNaming;
        
        foreach (var model in models)
        {
            Models[model.Type] = model;
        }
    }

    public int Count => Models.Count;

    public TypeResolver Resolver { get; }

    public static Collector Empty(TypeResolver? resolver = null) => new(
        resolver: resolver ?? new TypeResolver(default),
        enumNaming: NamingStrategy.Enum,
        propertyNaming: NamingStrategy.PascalCase,
        models: Array.Empty<Code>());

    [Pure]
    public Collector WithPropertyNaming(PropertyNaming strategy) => new(
        resolver: Resolver,
        enumNaming: EnumValueNaming,
        propertyNaming: strategy ?? NamingStrategy.PascalCase,
        models: Models.Values);


    [Pure]
    public Collector WithEnumNaming(EnumValueNaming strategy) => new(
        resolver: Resolver,
        enumNaming: strategy ?? NamingStrategy.None,
        propertyNaming: PropertyNaming,
        models: Models.Values);

    [Pure]
    public Collector Collect(IEnumerable<OpenApiNamedSchema> schemas)
    {
        Guard.NotNull(schemas, nameof(schemas));

        var collector = new Collector(
            resolver: Resolver,
            enumNaming: EnumValueNaming,
            propertyNaming: PropertyNaming,
            models: Models.Values);

        collector.ResolveTypes(schemas);
        collector.ResolveEnums(schemas);
        foreach (var property in schemas)
        {
            collector.Collect(property);
        }
        return collector;
    }

    [Pure]
    public Collector Collect(Stream stream)
    {
        var reader = new Microsoft.OpenApi.Readers.OpenApiStreamReader();
        var loaded = reader.Read(stream, out var diagnostic);

        foreach (var d in diagnostic.Errors)
        {
            Console.WriteLine($"ERR: {d.Message}");
        }
        foreach (var d in diagnostic.Warnings)
        {
            Console.WriteLine($"WRN: {d.Message}");
        }

        return Collect(loaded.Components.NamedSchemas());
    }

    private void Collect(OpenApiNamedSchema schema)
    {
        switch (schema.Type)
        {
            case OpenApiType.array:
            case OpenApiType.@object: Object(schema); break;

            case OpenApiType.None:
            case OpenApiType.@string:
            case OpenApiType.number:
            case OpenApiType.boolean:
            case OpenApiType.integer: /* Do not collect primitives. */ break;
            default: throw new NotSupportedException($"Schema  type '{schema.Type}' is not supported.");
        }

        void Object(OpenApiNamedSchema schema)
        {
            var type = Resolver.Resolve(schema);
            var model = new CodeModel(type, schema, schema.Properties.Select(prop => Property(prop, type)));
            foreach (var property in model)
            {
                property.Model = model;
            }
            Models[type] = model;
            
            foreach(var property in model)
            {
                var tp = property.Type.NotArray();
                if (!Models.ContainsKey(tp))
                {
                    if (tp.IsEnum)
                    {
                        ResolveEnum(type, property.Schema);
                    }
                    else
                    {
                        Collect(property.Schema);
                    }
                }
            }
        }

        CodeProperty Property(OpenApiNamedSchema schema, CodeType model)
        {
            var type = Resolver.Resolve(schema);
            var name = PropertyNaming(schema.Name, model);
            return new CodeProperty(type, name, schema);
        }
    }

    // Ensure all defined types are known before resolving child references.
    private void ResolveTypes(IEnumerable<OpenApiNamedSchema> schemas)
    {
        foreach (var schema in schemas.Where(s => !s.Schema.OneOf.Any())) _ = Resolver.Resolve(schema);
    }

    private void ResolveEnums(IEnumerable<OpenApiNamedSchema> schemas)
    {
        foreach (var schema in schemas)
        {
            if (Resolver.Resolve(schema) is { IsEnum: true } type) ResolveEnum(type, schema);
        }
    }
    private void ResolveEnum(CodeType type, OpenApiNamedSchema schema)
    {
        Models[type] = new CodeEnum(type, schema, schema.Enum.Select(AsNameValue));

        CodeNameValue AsNameValue(IOpenApiAny entry)
            => entry is OpenApiString str
            ? new CodeNameValue(EnumValueNaming(str.Value), str.Value, entry)
            : throw new NotSupportedException($"OpenApyAny of type '{entry.GetType()}' is not supported.");
    }

    public IEnumerator<Code> GetEnumerator() => Models.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
