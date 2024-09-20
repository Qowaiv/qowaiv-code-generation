using Microsoft.OpenApi.Any;
using Qowaiv.CodeGeneration.Syntax;
using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    [Pure]
    protected Type? Resolve(ResolveOpenApiSchema schema)
    {
        if (schema.ReferenceId is { } && resolved.TryGetValue(schema.ReferenceId, out var type))
        {
            return type;
        }
        else
        {
            type = ResolveCustomization(schema) ?? ResolveType(schema);
            if (schema.ReferenceId is { })
            {
                resolved.TryAdd(schema.ReferenceId, type);
            }
            return type;
        }
    }

    /// <summary>Custom resolving.</summary>
    [Pure]
    protected virtual Type? ResolveCustomization(ResolveOpenApiSchema schema) => null;

    /// <summary>Resolves the <see cref="Type"/> based on the <see cref="ResolveOpenApiSchema.OpenApiType"/>.</summary>
    [Pure]
    protected Type? ResolveType(ResolveOpenApiSchema schema) => schema.OpenApiType switch
    {
        _ when schema.Enum.Any() => ResolveEnum(schema),
        OpenApiType.boolean => ResolveBoolean(schema),
        OpenApiType.integer => ResolveInteger(schema),
        OpenApiType.number => ResolveNumber(schema),
        OpenApiType.@string => ResolveString(schema),
        OpenApiType.array => ResolveArray(schema),
        OpenApiType.@object => ResolveObject(schema),
        _ when schema.AllOf.Any() => ResolveAllOf(schema),
        _ when schema.OneOf.Any() => ResolveOneOf(schema),
        _ => ResolveOther(schema),
    };

    /// <summary>
    /// Resolves the <see cref="Type"/> when <see cref="ResolveOpenApiSchema.Enum"/>
    /// contains any items.
    /// </summary>
    [Pure]
    protected Type? ResolveEnum(ResolveOpenApiSchema schema)
    {
        if (ResolveName(schema, ResolveTypeName.Enum) is not { } nameType) return null;

        if (!EnumerationFields.TryGetValue(nameType, out var fields))
        {
            fields = new List<EnumerationField>();
            EnumerationFields[nameType] = fields;
        }

        if (!Enumerations.TryGetValue(nameType, out var type))
        {
            type = new Enumeration(new()
            {
                TypeName = nameType,
                Fields = fields,
                Visibility = ResolveVisibility(schema),
                Documentation = new XmlDocumentation() { Summary = schema.Description },
            });

            Enumerations[nameType] = type;
        }

        foreach (var @enum in schema.Enum)
        {
            if (@enum is OpenApiString str)
            {
                var field = ResolveEnumField(str, type);
                if (!fields.Exists(f => f.Name == field.Name))
                {
                    fields.Add(field);
                }
            }
        }
        if (!fields.Exists(IsNone))
        {
            fields.Insert(0, new(type, "None", 0, [AttributeInfo.System_Runtime_Serialization_EnumMember(null)]));
        }

        return type;

        static bool IsNone(EnumerationField field)
            => 0.Equals(field.Value)
            || (field.Name == "None" && field.Value is null);
    }

    private readonly Dictionary<TypeName, Enumeration> Enumerations = new();
    private readonly Dictionary<TypeName, List<EnumerationField>> EnumerationFields = new();

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.boolean"/>.</summary>
    [Pure]
    private static Type? ResolveBoolean(ResolveOpenApiSchema schema) => typeof(bool);

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.integer"/>.</summary>
    [Pure]
    private Type? ResolveInteger(ResolveOpenApiSchema schema) => NormalizeFormat(schema.Format) switch
    {
        "YEAR" => typeof(Year),
        "INT16" => typeof(short),
        "INT64" => typeof(long),
        _ => typeof(int),
    };

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.number"/>.</summary>
    [Pure]
    private Type? ResolveNumber(ResolveOpenApiSchema schema) => NormalizeFormat(schema.Format) switch
    {
        "INT16" => typeof(short),
        "INT32" => typeof(int),
        "INT64" => typeof(long),
        "AMOUNT" => typeof(Qowaiv.Financial.Amount),
        "ELO" => typeof(Qowaiv.Statistics.Elo),
        "PERCENTAGE" => typeof(Qowaiv.Percentage),
        _ => typeof(decimal),
    };

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.@string"/>.</summary>
    [Pure]
    private Type? ResolveString(ResolveOpenApiSchema schema)
        => Formats.TryGetValue(NormalizeFormat(schema.Format), out var type)
        ? type
        : typeof(string);

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.array"/>.</summary>
    [Pure]
    private Type? ResolveArray(ResolveOpenApiSchema schema)
        => Resolve(schema.Items)?.MakeArrayType();

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.@object"/>.</summary>
    [Pure]
    private Type? ResolveObject(ResolveOpenApiSchema schema)
    {
        var properties = new List<Property>();
        var derivedTypes = new List<Type>();
        var attributes = new List<AttributeInfo>();

        var baseType = schema.AllOf.Count() == 1
            ? ResolveObject(schema.AllOf.First())
            : schema.Base;

        var info = new TypeInfo
        {
            BaseType = baseType,
            TypeName = ResolveName(schema, ResolveTypeName.Object)!,
            IsSealed = Settings.Sealed,
            IsPartial = Settings.Partial,
            Properties = properties,
            DerivedTypes = derivedTypes,
            Attributes = attributes,
            Visibility = ResolveVisibility(schema),
            Documentation = new XmlDocumentation() { Summary = schema.Description },
        };

        Class classType = Settings.ModelType == ModelType.Record
            ? new Record(info)
            : new Class(info);

        schema = schema.WithModel(classType);

        properties.AddRange(schema.Properties.Select(p => ResolveProperty(p, schema.Required)).OfType<Property>());

        if (schema.AllOf.Count() > 1)
        {
            foreach (var reference in schema.AllOf)
            {
                properties.AddRange(reference.Properties
                    .Select(prop => ResolveProperty(prop, reference.Required))
                    .OfType<Property>());
            }
        }

        schema = schema.WithBase(classType);

        foreach (var oneOfSchema in schema.OneOf)
        {
            var derived = Resolve(oneOfSchema);
            if (derived is { })
            {
                derivedTypes.Add(derived);
            }
        }

        attributes.AddRange(DecorateModel(classType, schema));

        return classType;
    }

    /// <summary>
    /// Resolves the <see cref="Type"/> when there are multiple <see cref="ResolveOpenApiSchema.OneOf"/>'s.
    /// </summary>
    [Pure]
    private Type? ResolveOneOf(ResolveOpenApiSchema schema)
    {
        var allOff = schema.OneOf.SelectMany(o => o.AllOf).ToHashSet();

        if (allOff.Count == 1)
        {
            var child = allOff.Single();
            var type = Resolve(child);

            if (type is Class @class
                && @class.GetDerivedTypes() is List<Type> derivedTypes
                && @class.GetAttributeInfos() is List<AttributeInfo> infos)
            {
                derivedTypes.AddRange(schema.OneOf.Select(Resolve).OfType<Type>());
                infos.AddRange(derivedTypes.Select(d => DecorateDerivedType(d, schema)).OfType<AttributeInfo>());
            }
            return type;
        }
        else return null;
    }

    /// <summary>
    /// Resolves the <see cref="Type"/> when there is at least one <see cref="ResolveOpenApiSchema.AllOf"/>'s.
    /// </summary>
    [Pure]
    private Type? ResolveAllOf(ResolveOpenApiSchema schema)
    {
        var bases = schema.AllOf.Where(x => x.Reference is { }).ToArray();
        var inherit = schema.AllOf.Where(x => x.Reference is null && x.Properties.Any()).ToArray();

        if (bases.Length == 1)
        {
            if (inherit.Length == 0)
            {
                return Resolve(bases[0]);
            }
            else if (inherit.Length == 1
                && Resolve(bases[0]) is { } @base
                && Resolve(inherit[0].WithBase(@base)) is { } type)
            {
                AddDerivedType(@base, type, schema);

                return type;
            }
        }

        if (ResolveObject(schema) is Class @class && Properties(@class) is { } properties)
        {
            foreach (var @base in bases.Select(Resolve).OfType<Class>())
            {
                properties.AddRange(Properties(@base) ?? []);
            }
            return @class;
        }

        throw new NotSupportedException(
            $"Schema '{schema.Path}' with type multiple all-off is not supported for {{ {string.Join(", ", schema.AllOf.Select(s => $"'{s.Path}'"))} }}.");

        void AddDerivedType(Type @base, Type type, ResolveOpenApiSchema schema)
        {
            if (@base is Class @class
                && @class.GetDerivedTypes() is List<Type> derivedTypes
                && @class.GetAttributeInfos() is List<AttributeInfo> infos
                && !derivedTypes.Exists(t => t.FullName == type.FullName))
            {
                derivedTypes.Add(type);
                if (DecorateDerivedType(type, schema) is { } attr)
                {
                    infos.Add(attr);
                }
            }
        }

        List<Property>? Properties(Class @class) => typeof(Class)
            .GetField(nameof(Properties), (BindingFlags)36)?
            .GetValue(@class) as List<Property>;
    }

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiType.None"/>.</summary>
    [Pure]
    protected virtual Type? ResolveOther(ResolveOpenApiSchema schema)
        => throw new NotSupportedException($"Schema '{schema.Path}' with type '{schema.Type}' is not supported.");
}
