namespace Qowaiv.CodeGeneration.OpenApi;

public partial class OpenApiTypeResolver
{
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
