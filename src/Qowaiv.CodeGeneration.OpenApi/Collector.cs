using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace Qowaiv.OpenApi;

/// <summary>Collects TypeScript code files to generate.</summary>
/// <remarks>
/// This class is immutable. So every addition returns a new instance.
/// </remarks>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public sealed class Collector : IReadOnlyCollection<Code>
{
    private readonly Dictionary<DotNetType, Code> Models = new();
    private readonly TypeResolver Resolver;
    private readonly PropertyNamingStrategy PropertyNaming;
    private readonly Func<string, string> EnumNaming;

    private Collector(
        TypeResolver resolver,
        PropertyNamingStrategy propertyNaming,
        Func<string, string> enumNaming,
        IEnumerable<Code> models)
    {
        Resolver = resolver;
        PropertyNaming = propertyNaming;
        EnumNaming = enumNaming;
        foreach (var model in models)
        {
            Models[model.Type] = model;
        }
    }

    public int Count => Models.Count;

    public static Collector Empty(TypeResolver? resolver = null) => new(
        resolver: resolver ?? new TypeResolver(default),
        propertyNaming: NamingStrategy.PascalCase,
        enumNaming: NamingStrategy.Enum,
        models: Array.Empty<Code>());

    [Pure]
    public Collector WithPropertyNaming(PropertyNamingStrategy strategy) => new(
        resolver: Resolver,
        propertyNaming: strategy ?? NamingStrategy.PascalCase,
        enumNaming: EnumNaming,
        models: Models.Values);


    [Pure]
    public Collector WithEnumNaming(Func<string, string> strategy) => new(
        resolver: Resolver,
        propertyNaming: PropertyNaming,
        enumNaming: strategy ?? NamingStrategy.None,
        models: Models.Values);

    [Pure]
    public Collector Collect(IEnumerable<KeyValuePair<string, OpenApiSchema>> schemas)
    {
        Guard.NotNull(schemas, nameof(schemas));

        var collector = new Collector(Resolver, PropertyNaming, EnumNaming, Models.Values);

        collector.ResolveTypes(schemas);
        collector.ResolveEnums(schemas);
        foreach (var schema in schemas)
        {
            collector.Collect(schema.Key, schema.Value);
        }
        return collector;

       
    }

    public void Write(WriterSettings settings)
    {
        Guard.NotNull(settings, nameof(settings));

        DeleteExistingGeneratedFiles(settings);

        foreach (var model in this)
        {
            Write(model, settings);
        }
    }

    private static void DeleteExistingGeneratedFiles(WriterSettings settings)
    {
        if (!settings.DeleteExistingGeneratedFiles || !settings.RootLocation.Exists) return;

        var toDelete = settings.RootLocation.GetFiles("*.generated.cs", SearchOption.AllDirectories);
        foreach (var file in toDelete)
        {
            try
            {
                file.Delete();
            }
            catch (Exception x)
            {
                Console.WriteLine($"Could not delete '{file}': {x.Message}");
            }
        }

        var emptyDirectories = settings.RootLocation
            .EnumerateDirectories("*", SearchOption.AllDirectories)
            .Where(IsEmpty)
            .ToArray();

        foreach (var directory in emptyDirectories)
        {
            try
            {
                directory.Refresh();
                if (directory.Exists)
                {
                    directory.Delete(true);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine($"Could not delete '{directory}': {x.Message}");
            }
        }

        static bool IsEmpty(DirectoryInfo dir) => !dir.EnumerateFiles("*", SearchOption.AllDirectories).Any();
    }

    private void Write(Code code, WriterSettings settings)
    {
        var ns = code.Type.Namespace.Name;

        if (ns == Resolver.DefaultNamespace.Name || ns.StartsWith($"{Resolver.DefaultNamespace.Name}."))
        {
            ns = ns[Resolver.DefaultNamespace.Name.Length..].TrimStart('.').Replace('.', '\\');
        }

        var path = Path.Combine(settings.RootLocation.FullName, ns, code.Type.Name + ".generated.cs");

        var location = new FileInfo(path);
        if (!location.Directory!.Exists)
        {
            location.Directory.Create();
        }
        using var writer = new StreamWriter(location.FullName, CSharpWriter.Encoding, new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create });

        writer
            .AddIsGeneratedCodeHeader()
            .Line()
            .Line(code.Type.Namespace)
            .Line()
            .Add(code, settings);
    }

    private void Collect(string name, OpenApiSchema schema)
    {
        switch (schema.Type)
        {
            case "object": Object(name, schema); break;

            case "array":
            case "string":
            case "integer": /* Do not collect primitives. */ break;
            default: throw new NotSupportedException($"Schema  type '{schema.Type}' is not supported.");
        }

        void Object(string name, OpenApiSchema schema)
        {
            var type = Resolver.Resolve(name, schema);
            Models.Add(type, new Model
            {
                Type = type,
                Properties = schema.Properties.Select(kvp => Property(kvp.Key, kvp.Value, type)).ToArray(),
            });
        }

        Property Property(string name, OpenApiSchema schema, DotNetType model)
        {
            var type = Resolver.Resolve(name, schema);
            return new Property(type, PropertyNaming(name, model), name, new PropertyInfo
            {
                Type = schema.Type,
                Format = schema.Format,
                Pattern = schema.Pattern,
                Description = Descriptor(schema.Description),
            });
        }

        static string? Descriptor(string? desc) => desc?.TrimEnd('\n').TrimEnd('\r');
    }

    // Ensure all defined types are known before resolving child references.
    private void ResolveTypes(IEnumerable<KeyValuePair<string, OpenApiSchema>> schemas)
    {
        foreach (var schema in schemas)
        {
            _ = Resolver.Resolve(schema.Key, schema.Value);
        }
    }

    private void ResolveEnums(IEnumerable<KeyValuePair<string, OpenApiSchema>> schemas)
    {
        foreach (var schema in schemas)
        {
            var type = Resolver.Resolve(schema.Key, schema.Value);
            if (type.IsEnum)
            {
                ResolveEnum(type, schema.Value);
            }
        }

        void ResolveEnum(DotNetType type, OpenApiSchema schema)
        {
            Models.Add(type, new Enumeration
            {
                Type = type,
                Values = schema.Enum.Select(AsEnumValue).ToArray(),
            });

            EnumValue AsEnumValue(IOpenApiAny entry)
                => entry is OpenApiString str
                ? new EnumValue(EnumNaming(str.Value), str.Value)
                : throw new NotSupportedException($"OpenApyAny of type '{entry.GetType()}' is not supported.");
        }
    }

    public IEnumerator<Code> GetEnumerator() => Models.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
