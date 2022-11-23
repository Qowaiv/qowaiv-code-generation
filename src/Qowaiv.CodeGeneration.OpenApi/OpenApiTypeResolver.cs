using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Qowaiv.CodeGeneration.Syntax;
using System.IO;
using System.Reflection;

namespace Qowaiv.CodeGeneration.OpenApi;

public class OpenApiTypeResolver
{
    public OpenApiTypeResolver(Namespace defaultNamespace, OpenApiTypeResolverSettings? settings = null, Decorator? decorator = null)
    {
        DefaultNamespace = Guard.NotDefault(defaultNamespace, nameof(defaultNamespace));
        Settings = settings ?? new OpenApiTypeResolverSettings();
        Decorator = decorator ?? new Decorator();
    }

    public Namespace DefaultNamespace { get; }
    private readonly Decorator Decorator;
    private readonly OpenApiTypeResolverSettings Settings;

    [Pure]
    public IEnumerable<Code> Resolve(FileInfo documentLocation, out OpenApiDiagnostic diagnostic)
    {
        using var stream = documentLocation.OpenRead();
        return Resolve(stream, out diagnostic);
    }

    [Pure]
    public IEnumerable<Code> Resolve(Stream documentStream, out OpenApiDiagnostic diagnostic)
    {
        var reader = new OpenApiStreamReader();
        var document = reader.Read(documentStream, out diagnostic);
        return Resolve(document);
    }

    /// <summary>Resolves the 
    /// 
    /// </summary>
    /// <param name="document"></param>
    /// <returns></returns>
    [Pure]
    public IEnumerable<Code> Resolve(OpenApiDocument document)
        => Guard.NotNull(document, nameof(document))
        .Components.Schemas.Values.Select(Resolve)
        .OfType<Code>();

    [Pure]
    private Type? Resolve(OpenApiSchema schema)
    {
        var id = schema.Reference?.Id;

        if (id is { } && Resolved.TryGetValue(id, out var type))
        {
            return type;
        }
        else
        {
            type = Custom(schema) ?? ResolveType(schema);

            if (id is { }) Resolved[id] = type;

            return type;
        }
    }

    [Pure]
    protected virtual Type? Custom(OpenApiSchema schema) => null;

    [Pure]
    protected virtual PropertyAccess Access(OpenApiProperty property) => PropertyAccess.InitOnly;

    /// <remarks>
    /// Types that are null-able by design will not be transformed to 
    /// <see cref="Nullable{T}"/>'s when optional.
    /// </remarks>
    [Pure]
    public virtual bool IsNullableByDesign(Type type) 
        => type.IsValueType && type
            .GetFields(BindingFlags.Static | BindingFlags.Public)
            .Any(f => f.FieldType == type && (f.Name == "Empty" || f.Name == "None"));

    [Pure]
    protected virtual Type? ResolveType(OpenApiSchema schema)
        => schema.OpenApiType() switch
        {
            OpenApiType.None => null,
            OpenApiType.boolean => typeof(bool),
            OpenApiType.integer => ResolveInteger(schema),
            OpenApiType.number => ResolveNumber(schema),
            OpenApiType.@string => ResolveString(schema),
            OpenApiType.array => Resolve(schema.Items)?.MakeArrayType(),
            OpenApiType.@object => ResolveObject(schema),
            _ => throw new NotSupportedException($"Schema with type '{schema.Type}' is not supported."),
        };

    [Pure]
    protected virtual Type ResolveInteger(OpenApiSchema schema)
        => Normalize(schema.Format) switch
        {
            "YEAR" => typeof(Year),
            _ => typeof(int),
        };

    [Pure]
    protected virtual Type ResolveNumber(OpenApiSchema schema)
       => Normalize(schema.Format) switch
       {
           "AMOUNT" => typeof(Qowaiv.Financial.Amount),
           "ELO" => typeof(Qowaiv.Statistics.Elo),
           _ => typeof(decimal),
       };

    [Pure]
    protected virtual Type? ResolveString(OpenApiSchema schema)
        => Normalize(schema.Format) switch
        {
            "BIC" => typeof(Qowaiv.Financial.BusinessIdentifierCode),
            "COUNTRY" => typeof(Qowaiv.Globalization.Country),
            "CURRENCY" => typeof(Qowaiv.Financial.Currency),
            "DATE" => typeof(System.DateOnly),
            "DATESPAN" => typeof(Qowaiv.DateSpan),
            "DATETIME" => typeof(System.DateTime),
            "DATEWEEKBASED" => typeof(Qowaiv.WeekDate),
            "EMAIL" => typeof(Qowaiv.EmailAddress),
            "EMAILCOLLECTION" => typeof(Qowaiv.EmailAddressCollection),
            "FRACTION" => typeof(Qowaiv.Mathematics.Fraction),
            "GUID" or "UUID" => typeof(System.Guid),
            "IBAN" => typeof(Qowaiv.Financial.InternationalBankAccountNumber),
            "HOUSENUMBER" => typeof(Qowaiv.HouseNumber),
            "LOCALDATETIME" => typeof(Qowaiv.LocalDateTime),
            "MONEY" => typeof(Qowaiv.Financial.Money),
            "MONTH" => typeof(Qowaiv.Month),
            "MONTHSPAN" => typeof(Qowaiv.MonthSpan),
            "PERCENTAGE" => typeof(Qowaiv.Percentage),
            "POSTALCODE" => typeof(Qowaiv.PostalCode),
            "GENDER" or "SEX" => typeof(Qowaiv.Sex),
            "STREAMSIZE" => typeof(Qowaiv.IO.StreamSize),
            "UUIDBASE64" => typeof(Qowaiv.Uuid),
            "YESNO" => typeof(Qowaiv.YesNo),

            _ => schema.Enum.Any() ? ResolveEnum(schema) : typeof(string),
        };


    [Pure]
    protected virtual Type? ResolveEnum(OpenApiSchema schema)
    {
        if (ResolveName(schema) is not { } nameType) return null;
        var fields = new List<EnumerationField>();
        var type = new Enumeration(new() { TypeName = nameType, Fields = fields });

        foreach (var @enum in schema.Enum)
        {
            if (@enum is OpenApiString str)
            {
                fields.Add(new(type, str.Value, null));
            }
        }
        return type;
    }

    [Pure]
    protected virtual Type ResolveObject(OpenApiSchema schema)
    {
        var properties = new List<Property>();
        var settings = new Syntax.TypeInfo
        {
            TypeName = ResolveName(schema)!,
            IsSealed = Settings.Sealed,
            IsPartial = Settings.Partial,
            Properties = properties,
        };

        Class classType = Settings.ModelType == ModelType.Record
            ? new Record(settings)
            : new Class(settings);

        properties.AddRange(schema.OpenApiProperties().Select(prop => Resolve(classType, prop)).OfType<Property>());

        return classType;
    }

    [Pure]
    private Property? Resolve(Class @class, OpenApiProperty property)
    {
        var propertyType = Resolve(property.Schema);
        if (propertyType is { })
        {
            // nullablillity is handled on type for structs, and on the property for classes.
            var nullable = propertyType.IsClass && !propertyType.IsArray;

            if (property.Schema.Nullable && propertyType.IsValueType && !IsNullableByDesign(propertyType))
            {
                propertyType = typeof(Nullable<>).MakeGenericType(propertyType);
            }

            var attributes = new List<AttributeInfo>();
            var documentation = new XmlDocumentation
            {
                Summary = property.Schema.Description,
            };
            var prop = new Property(
                PorpertyName(@class, property),
                propertyType,
                @class, 
                Access(property), 
                attributes, 
                documentation,
                nullable);

            attributes.AddRange(Decorator.Property(prop, property));
            return prop;
        }
        else return null;
    }

    [Pure]
    protected virtual string PorpertyName(Class @class, OpenApiProperty property)
        => NamingStrategy.PascalCase(property.Name, @class);

    [Pure]
    protected virtual TypeName? ResolveName(OpenApiSchema schema)
    {
        if (schema.Reference?.Id is not { } name) return null;

        var lastDot = name.LastIndexOf('.');
        var ns = lastDot == -1 ? DefaultNamespace : DefaultNamespace.Child(name[..lastDot]);
        name = NamingStrategy.PascalCase((lastDot == -1 ? name : name[(lastDot + 1)..]).TrimStart('_'));
        return new(ns, name);
    }

    [Pure]
    protected virtual string Normalize(string? str)
        => (str ?? string.Empty).ToUpperInvariant().Replace("-", "");

    private readonly Dictionary<string, Type?> Resolved = new();
}
