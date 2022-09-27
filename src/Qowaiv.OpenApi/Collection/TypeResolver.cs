namespace Qowaiv.OpenApi.Collection;

public class TypeResolver
{
    public TypeResolver(Namespace defaultNamespace) => DefaultNamespace = defaultNamespace;

    public Namespace DefaultNamespace { get; }

    [Pure]
    public CodeType Resolve(OpenApiNamedSchema? schema)
    {
        if (schema is null) return CodeType.Object;
        if (!Resolved.TryGetValue(schema, out var resolved))
        {
            resolved = Custom(schema) ?? ResolveType(schema);
            Resolved[schema] = resolved;
        }
        return resolved;
    }

    [Pure]
    public virtual CodeType? Custom(OpenApiNamedSchema schema) => null;

    [Pure]
    private CodeType ResolveType(OpenApiNamedSchema schema) 
        => schema.Type switch
        {
            OpenApiType.integer => ResolveInteger(schema),
            OpenApiType.number => ResolveNumber(schema),
            OpenApiType.@string => ResolveString(schema),
            OpenApiType.array => Resolve(schema.Items).Array(),
            OpenApiType.None or OpenApiType.@object => ResolveObject(schema),
            OpenApiType.boolean => CodeType.Boolean,
            _ => throw new NotSupportedException($"Schema with type '{schema.Type}' is not supported."),
        };

    [Pure]
    private static CodeType ResolveInteger(OpenApiNamedSchema schema)
        => Normalize(schema.Format) switch
        {
            "YEAR" => CodeType.Year,
            _ => CodeType.Int32,
        };

    [Pure]
    private static CodeType ResolveNumber(OpenApiNamedSchema schema)
       => Normalize(schema.Format) switch
       {
           "AMOUNT" => CodeType.Amount,
           "ELO" => CodeType.Elo,
           _ => CodeType.Decimal,
       };

    [Pure]
    private CodeType ResolveString(OpenApiNamedSchema schema)
        => Normalize(schema.Format) switch
        {
            "BIC" => CodeType.BusinessIdentifierCode,
            "COUNTRY" => CodeType.Country,
            "CURRENCY" => CodeType.Currency,
            "DATE" => CodeType.DateOnly,
            "DATESPAN" => CodeType.DateSpan,
            "DATETIME" => CodeType.DateTime,
            "DATEWEEKBASED" => CodeType.WeekDate,
            "EMAIL" => CodeType.EmailAddress,
            "EMAILCOLLECTION" => CodeType.EmailAddressCollection,
            "FRACTION" => CodeType.Fraction,
            "GUID" or "UUID" => CodeType.Guid,
            "IBAN" => CodeType.InternationalBankAccountNumber,
            "HOUSENUMBER" => CodeType.HouseNumber,
            "LOCALDATETIME" => CodeType.LocalDateTime,
            "MONEY" => CodeType.Money,
            "MONTH" => CodeType.Month,
            "MONTHSPAN" => CodeType.MonthSpan,
            "PERCENTAGE" => CodeType.Percentage,
            "POSTALCODE" => CodeType.Percentage,
            "GENDER" or "SEX" => CodeType.Sex,
            "STREAMSIZE" => CodeType.StreamSize,
            "UUIDBASE64" => CodeType.Uuid,
            "YESNO" => CodeType.YesNo,

            _ => schema.Enum.Any() ? ResolveEnum(schema) : ResolveString(schema.Name),
        };

    [Pure]
    private static CodeType ResolveString(string name)
    {
        if (new[] { "EMAIL", "EMAILADDRESS" }.Contains(name.ToUpperInvariant())) return CodeType.EmailAddress;
        else if (new[] { "IBAN", "INTERNATIONALBANKACCOUNTNUMBER" }.Contains(name.ToUpperInvariant())) return CodeType.InternationalBankAccountNumber;
        else return CodeType.String;
    }

    [Pure]
    private CodeType ResolveEnum(OpenApiNamedSchema schema) => ResolveName(schema).Enum();

    [Pure]
    private CodeType ResolveObject(OpenApiNamedSchema schema) => ResolveName(schema);

    [Pure]
    private CodeType ResolveName(OpenApiNamedSchema schema)
    {
        var name = schema.Name;
        var lastDot = name.LastIndexOf('.');
        var ns = lastDot == -1 ? DefaultNamespace : DefaultNamespace.Child(name[..lastDot]);
        name = NamingStrategy.PascalCase((lastDot == -1 ? name : name[(lastDot + 1)..]).TrimStart('_'));
        return new(name, ns);
    }

    [Pure]
    private static string Normalize(string? str) 
        => (str ?? string.Empty).ToUpperInvariant().Replace("-", "");

    private static readonly Dictionary<OpenApiNamedSchema, CodeType> Resolved = new();
}

