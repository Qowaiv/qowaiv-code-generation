using Microsoft.OpenApi.Any;
using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.array"/>.</summary>
    [Pure]
    private Type? ResolveArray(ResolveOpenApiSchema schema)
        => schema.Items is { } items
        ? Resolve(items)?.MakeArrayType()
        : null;

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.boolean"/>.</summary>
    [Pure]
    private static Type? ResolveBoolean(ResolveOpenApiSchema schema) => typeof(bool);

    /// <summary>
    /// Resolves the <see cref="Type"/> when <see cref="ResolveOpenApiSchema.Enum"/>
    /// contains any items.
    /// </summary>
    [Pure]
    protected Enumeration? ResolveEnum(ResolveOpenApiSchema schema)
    {
        if (ResolveName(schema, ResolveTypeName.Enum) is not { } nameType) return null;

        if (!EnumerationFields.TryGetValue(nameType, out var fields))
        {
            fields = [];
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

    private readonly Dictionary<TypeName, Enumeration> Enumerations = [];
    private readonly Dictionary<TypeName, List<EnumerationField>> EnumerationFields = [];

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.integer"/>.</summary>
    [Pure]
    private Type? ResolveInteger(ResolveOpenApiSchema schema) => NormalizeFormat(schema.Format) switch
    {
        "YEAR" => typeof(Year),
        "INT16" => typeof(short),
        "INT64" => typeof(long),
        _ => typeof(int),
    };

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.number"/>.</summary>
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

    /// <summary>Resolves the <see cref="Type"/> for <see cref="OpenApiDataType.@string"/>.</summary>
    [Pure]
    private Type? ResolveString(ResolveOpenApiSchema schema)
        => Formats.TryGetValue(NormalizeFormat(schema.Format), out var type)
        ? type
        : typeof(string);

    private static readonly IReadOnlyDictionary<string, Type> Formats = new Dictionary<string, Type>
    {
        ["BIC"] = typeof(Qowaiv.Financial.BusinessIdentifierCode),
        ["BYTE"] = typeof(System.BinaryData),
        ["COUNTRY"] = typeof(Qowaiv.Globalization.Country),
        ["CURRENCY"] = typeof(Qowaiv.Financial.Currency),
        ["DATE"] = typeof(System.DateOnly),
        ["DATESPAN"] = typeof(Qowaiv.DateSpan),
        ["DATETIME"] = typeof(System.DateTime),
        ["DATEWEEKBASED"] = typeof(Qowaiv.WeekDate),
        ["EMAIL"] = typeof(Qowaiv.EmailAddress),
        ["ENERGYLABEL"] = typeof(Qowaiv.Sustainability.EnergyLabel),
        ["FRACTION"] = typeof(Qowaiv.Mathematics.Fraction),
        ["GUID"] = typeof(System.Guid),
        ["UUID"] = typeof(System.Guid),
        ["IBAN"] = typeof(Qowaiv.Financial.InternationalBankAccountNumber),
        ["HOUSENUMBER"] = typeof(Qowaiv.HouseNumber),
        ["LOCALDATETIME"] = typeof(Qowaiv.LocalDateTime),
        ["MONEY"] = typeof(Qowaiv.Financial.Money),
        ["MONTH"] = typeof(Qowaiv.Month),
        ["MONTHSPAN"] = typeof(Qowaiv.MonthSpan),
        ["PERCENTAGE"] = typeof(Qowaiv.Percentage),
        ["POSTALCODE"] = typeof(Qowaiv.PostalCode),
        ["GENDER"] = typeof(Qowaiv.Sex),
        ["SEX"] = typeof(Qowaiv.Sex),
        ["STREAMSIZE"] = typeof(Qowaiv.IO.StreamSize),
        ["URI"] = typeof(System.Uri),
        ["UUIDBASE64"] = typeof(Qowaiv.Uuid),
        ["YESNO"] = typeof(Qowaiv.YesNo),
    };
}
