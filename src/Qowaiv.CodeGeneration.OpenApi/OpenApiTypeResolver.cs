using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Qowaiv.CodeGeneration.Syntax;
using System.IO;

namespace Qowaiv.CodeGeneration.OpenApi;

public class OpenApiTypeResolver
{
    public OpenApiTypeResolver(Namespace defaultNamespace, Decorator? decorator = null)
    {
        DefaultNamespace = Guard.NotDefault(defaultNamespace, nameof(defaultNamespace));
        Decorator = decorator ?? new Decorator();
    }

    public Namespace DefaultNamespace { get; }
    private readonly Decorator Decorator;

    [Pure]
    public IEnumerable<Type> Resolve(FileInfo documentLocation, out OpenApiDiagnostic diagnostic)
    {
        using var stream = documentLocation.OpenRead();
        return Resolve(stream, out diagnostic);
    }

    [Pure]
    public IEnumerable<Type> Resolve(Stream documentStream, out OpenApiDiagnostic diagnostic)
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
    public IEnumerable<Type> Resolve(OpenApiDocument document)
        => Guard.NotNull(document, nameof(document))
        .Components.Schemas.Values.Select(schema => Resolve(schema))
        .OfType<ObjectBase>();

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
        var type = new Enumeration(nameType, fields);

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
        var nameType = ResolveName(schema)!;
        var properties = new List<Property>();
        var classType = new Class(nameType, properties: properties);

        properties.AddRange(schema.OpenApiProperties().Select(prop => Resolve(classType, prop)).OfType<Property>());

        return classType;
    }

    [Pure]
    private Property? Resolve(Class @class, OpenApiProperty property)
    {
        var propertyType = Resolve(property.Schema);
        if (propertyType is { })
        {
            var attributes = new List<AttributeInfo>();
            var documentation = new XmlDocumentation
            {
                Summary = "TODO",
            };
            var prop = new Property(PorpertyName(@class, property), propertyType, @class, Access(property), attributes, documentation);
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
