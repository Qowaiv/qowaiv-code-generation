using Microsoft.OpenApi.Models;

namespace Qowaiv.OpenApi;

public class TypeResolver
{
    public TypeResolver(Namespace defaultNamespace) => DefaultNamespace = defaultNamespace;

    public Namespace DefaultNamespace { get; }

    [Pure]
    public DotNetType Resolve(string name, OpenApiSchema schema)
    {
        if (!Resolved.TryGetValue(schema, out var resolved))
        {
            resolved = Custom(name, schema) ?? ResolveType(name, schema);
            Resolved[schema] = resolved;
        }
        return resolved;
    }

    [Pure]
    public virtual DotNetType? Custom(string name, OpenApiSchema schema) => null;

    [Pure]
    private DotNetType ResolveType(string name, OpenApiSchema schema) 
        => schema.Type switch
        {
            "integer" => ResolveInteger(schema),
            "number" => ResolveNumber(schema),
            "string" => ResolveString(name, schema),
            "array" => Resolve(name, schema.Items).Array(),
            "object" => ResolveObject(name, schema),
            "boolean" => DotNetType.Boolean,
            _ => throw new NotSupportedException($"Schema with type '{schema.Type}' is not supported."),
        };

    [Pure]
    private static DotNetType ResolveInteger(OpenApiSchema schema)
        => Normalize(schema.Format) switch
        {
            "YEAR" => DotNetType.Year,
            _ => DotNetType.Int32,
        };

    [Pure]
    private static DotNetType ResolveNumber(OpenApiSchema schema)
       => Normalize(schema.Format) switch
       {
           "AMOUNT" => DotNetType.Amount,
           "ELO" => DotNetType.Elo,
           _ => DotNetType.Decimal,
       };

    [Pure]
    private DotNetType ResolveString(string name, OpenApiSchema schema)
        => Normalize(schema.Format) switch
        {
            "BIC" => DotNetType.BusinessIdentifierCode,
            "COUNTRY" => DotNetType.Country,
            "CURRENCY" => DotNetType.Currency,
            "DATE" => DotNetType.DateOnly,
            "DATESPAN" => DotNetType.DateSpan,
            "DATETIME" => DotNetType.DateTime,
            "DATEWEEKBASED" => DotNetType.WeekDate,
            "EMAIL" => DotNetType.EmailAddress,
            "EMAILCOLLECTION" => DotNetType.EmailAddressCollection,
            "FACTION" => DotNetType.Faction,
            "GUID" or "UUID" => DotNetType.Guid,
            "IBAN" => DotNetType.InternationalBankAccountNumber,
            "HOUSENUMBER" => DotNetType.HouseNumber,
            "LOCALDATETIME" => DotNetType.HouseNumber,
            "MONEY" => DotNetType.Money,
            "MONTH" => DotNetType.Month,
            "MONTHSPAN" => DotNetType.MonthSpan,
            "PERCENTAGE" => DotNetType.Percentage,
            "POSTALCODE" => DotNetType.Percentage,
            "SEX" => DotNetType.Sex,
            "STREAMSIZE" => DotNetType.StreamSize,
            "UUIDBASE64" => DotNetType.Uuid,
            "YESNO" => DotNetType.YesNo,

            _ => schema.Enum.Any() ? ResolveEnum(name) : DotNetType.String,
        };

    [Pure]
    private DotNetType ResolveEnum(string name) => ResolveType(name).Enum();

    [Pure]
    private DotNetType ResolveObject(string name, OpenApiSchema schema) => ResolveType(name);

    [Pure]
    public DotNetType ResolveType(string name)
    {
        var lastDot = name.LastIndexOf('.');
        return lastDot == -1
            ? new(name, DefaultNamespace)
            : new(name[(lastDot + 1)..], DefaultNamespace.Child(name[..lastDot]));
    }

    [Pure]
    private static string Normalize(string? str) 
        => (str ?? string.Empty).ToUpperInvariant().Replace("-", "");

    private static readonly Dictionary<OpenApiSchema, DotNetType> Resolved = new();
}

