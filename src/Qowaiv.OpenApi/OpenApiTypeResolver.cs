using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Qowaiv.CodeGeneration;
using Qowaiv.OpenApi.Collection;
using Qowaiv.OpenApi.Decorators;
using System.IO;

namespace Qowaiv.OpenApi;

public class OpenApiTypeResolver
{
    public OpenApiTypeResolver(Namespace defaultNamespace, params CodeDecorator[] decorators)
    {
        DefaultNamespace = Guard.NotDefault(defaultNamespace, nameof(defaultNamespace));
        Decorators = decorators ?? Array.Empty<CodeDecorator>();
    }

    public Namespace DefaultNamespace { get; }
    public IReadOnlyCollection<CodeDecorator> Decorators { get; }

    [Pure]
    public IEnumerable<TypeInfo> Resolve(FileInfo documentLocation, out OpenApiDiagnostic diagnostic)
    {
        using var stream = documentLocation.OpenRead();
        return Resolve(stream, out diagnostic);
    }

    [Pure]
    public IEnumerable<TypeInfo> Resolve(Stream documentStream, out OpenApiDiagnostic diagnostic)
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
    public IEnumerable<TypeInfo> Resolve(OpenApiDocument document)
        => Guard.NotNull(document, nameof(document))
        .Components.Schemas.Values.Select(schema => Resolve(schema))
        .OfType<TypeInfo>();

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
            OpenApiType.boolean => TypeInfo.Boolean,
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
            "YEAR" => TypeInfo.Year,
            _ => TypeInfo.Int32,
        };

    [Pure]
    protected virtual Type ResolveNumber(OpenApiSchema schema)
       => Normalize(schema.Format) switch
       {
           "AMOUNT" => TypeInfo.Amount,
           "ELO" => TypeInfo.Elo,
           _ => TypeInfo.Decimal,
       };

    [Pure]
    protected virtual Type? ResolveString(OpenApiSchema schema)
        => Normalize(schema.Format) switch
        {
            "BIC" => TypeInfo.BusinessIdentifierCode,
            "COUNTRY" => TypeInfo.Country,
            "CURRENCY" => TypeInfo.Currency,
            "DATE" => TypeInfo.DateOnly,
            "DATESPAN" => TypeInfo.DateSpan,
            "DATETIME" => TypeInfo.DateTime,
            "DATEWEEKBASED" => TypeInfo.WeekDate,
            "EMAIL" => TypeInfo.EmailAddress,
            "EMAILCOLLECTION" => TypeInfo.EmailAddressCollection,
            "FRACTION" => TypeInfo.Fraction,
            "GUID" or "UUID" => TypeInfo.Guid,
            "IBAN" => TypeInfo.InternationalBankAccountNumber,
            "HOUSENUMBER" => TypeInfo.HouseNumber,
            "LOCALDATETIME" => TypeInfo.LocalDateTime,
            "MONEY" => TypeInfo.Money,
            "MONTH" => TypeInfo.Month,
            "MONTHSPAN" => TypeInfo.MonthSpan,
            "PERCENTAGE" => TypeInfo.Percentage,
            "POSTALCODE" => TypeInfo.Percentage,
            "GENDER" or "SEX" => TypeInfo.Sex,
            "STREAMSIZE" => TypeInfo.StreamSize,
            "UUIDBASE64" => TypeInfo.Uuid,
            "YESNO" => TypeInfo.YesNo,

            _ => schema.Enum.Any() ? ResolveEnum(schema) : TypeInfo.String,
        };


    [Pure]
    protected virtual Type? ResolveEnum(OpenApiSchema schema)
    {
        var nameType = ResolveName(schema);
        var fields = new List<EnumerationField>();
        var type = new Enumeration(nameType, fields);

        foreach(var @enum in schema.Enum)
        {
            if (@enum is OpenApiString str)
            {
                fields.Add(new(type, str.Value, str.Value, null));
            }
        }
        return type;
    }

    [Pure]
    protected virtual Type ResolveObject(OpenApiSchema schema)
    {
        var nameType = ResolveName(schema);
        var properties = new List<Property>();
        var classType = new Class(nameType, properties);

        properties.AddRange(schema.OpenApiProperties().Select(prop => Resolve(classType, prop)).OfType<Property>());

        return classType;
    }

    [Pure]
    private Property? Resolve(Class @class, OpenApiProperty property)
    {
        var propertyType = Resolve(property.Schema);
        if (propertyType is { })
        {
            var decorations = new List<Code>();
            var prop = new Property(PorpertyName(@class, property), propertyType, @class, Access(property), decorations);
            decorations.AddRange(Decorators.SelectMany(d => d.Property(prop, property)));
            return prop;
        }
        else return null;
    }

    [Pure]
    protected virtual string PorpertyName(Class @class, OpenApiProperty property)
        => NamingStrategy.PascalCase(property.Name, @class);

    [Pure]
    protected virtual TypeName ResolveName(OpenApiSchema schema)
    {
        var name = schema.Reference.Id;
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
