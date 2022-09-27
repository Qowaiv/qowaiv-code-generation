namespace Qowaiv.OpenApi;

public record DotNetType(string Name, Namespace Namespace)
{
    public static readonly DotNetType Object = new("Object", "System") { Alias = "object" };

    public static readonly DotNetType String = new("String", "System") { Alias = "string" };
    public static readonly DotNetType Boolean = new("Boolean", "System") { Alias = "bool" };

    public static readonly DotNetType Decimal = new("Decimal", "System") { Alias = "decimal" };
    public static readonly DotNetType Int32 = new("Int32", "System") { Alias = "int" };

    public static readonly DotNetType Guid = new("Guid", "System");
    public static readonly DotNetType DateOnly = new("DateOnly", "System");
    public static readonly DotNetType DateTime = new("DateTime", "System");

    public static readonly DotNetType Date = new("Date", "Qowaiv");
    public static readonly DotNetType DateSpan = new("DateSpan", "Qowaiv");
    public static readonly DotNetType EmailAddress = new("EmailAddress", "Qowaiv");
    public static readonly DotNetType EmailAddressCollection = new("EmailAddressCollection", "Qowaiv");
    public static readonly DotNetType HouseNumber = new("HouseNumber", "Qowaiv");
    public static readonly DotNetType LocalDateTime = new("LocalDateTime", "Qowaiv");
    public static readonly DotNetType Month = new("Month", "Qowaiv");
    public static readonly DotNetType MonthSpan = new("MonthSpan", "Qowaiv");
    public static readonly DotNetType Percentage = new("Percentage", "Qowaiv");
    public static readonly DotNetType PostalCode = new("Percentage", "Qowaiv.PostalCode");
    public static readonly DotNetType Sex = new("Sex", "Qowaiv");
    public static readonly DotNetType Uuid = new("Uuid", "Qowaiv");
    public static readonly DotNetType WeekDate = new("WeekDate", "Qowaiv");
    public static readonly DotNetType Year = new("Year", "Qowaiv");
    public static readonly DotNetType YesNo = new("YesNo", "Qowaiv");

    public static readonly DotNetType Amount = new("Amount", "Qowaiv.Financial");
    public static readonly DotNetType BusinessIdentifierCode = new("BusinessIdentifierCode", "Qowaiv.Financial");
    public static readonly DotNetType Currency = new("Currency", "Qowaiv.Financial");
    public static readonly DotNetType InternationalBankAccountNumber = new("InternationalBankAccountNumber", "Qowaiv.Financial");
    public static readonly DotNetType Money = new("Money", "Qowaiv.Financial");

    public static readonly DotNetType Country = new("Country", "Qowaiv.Globalization");

    public static readonly DotNetType StreamSize = new("StreamSize", "Qowaiv.IO");

    public static readonly DotNetType Faction = new("Faction", "Qowaiv.Mathematics");

    public static readonly DotNetType Elo = new("StreamSize", "Qowaiv.Statistics");

    public string? Alias { get; init; }
    public bool IsArray { get; init; }
    public bool IsEnum { get; init; }

    public override string ToString() 
        => $"{Alias ?? $"{Namespace}.{Name}"}{(IsArray ? "[]" : "")}";

    public DotNetType Array() => this with { IsArray = true };

    public DotNetType Enum() => this with { IsEnum = true };
}

