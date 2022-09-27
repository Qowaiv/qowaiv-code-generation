namespace Qowaiv.OpenApi;

public record CodeType(string Name, Namespace Namespace)
{
    public static readonly CodeType Object = new("Object", "System") { Alias = "object" };

    public static readonly CodeType String = new("String", "System") { Alias = "string" };
    public static readonly CodeType Boolean = new("Boolean", "System") { Alias = "bool" };

    public static readonly CodeType Decimal = new("Decimal", "System") { Alias = "decimal" };
    public static readonly CodeType Int32 = new("Int32", "System") { Alias = "int" };
    public static readonly CodeType Int64 = new("Int64", "System") { Alias = "long" };

    public static readonly CodeType Guid = new("Guid", "System");
    public static readonly CodeType DateOnly = new("DateOnly", "System");
    public static readonly CodeType DateTime = new("DateTime", "System");

    public static readonly CodeType Date = new("Date", "Qowaiv");
    public static readonly CodeType DateSpan = new("DateSpan", "Qowaiv");
    public static readonly CodeType EmailAddress = new("EmailAddress", "Qowaiv");
    public static readonly CodeType EmailAddressCollection = new("EmailAddressCollection", "Qowaiv");
    public static readonly CodeType HouseNumber = new("HouseNumber", "Qowaiv");
    public static readonly CodeType LocalDateTime = new("LocalDateTime", "Qowaiv");
    public static readonly CodeType Month = new("Month", "Qowaiv");
    public static readonly CodeType MonthSpan = new("MonthSpan", "Qowaiv");
    public static readonly CodeType Percentage = new("Percentage", "Qowaiv");
    public static readonly CodeType PostalCode = new("Percentage", "Qowaiv.PostalCode");
    public static readonly CodeType Sex = new("Sex", "Qowaiv");
    public static readonly CodeType Uuid = new("Uuid", "Qowaiv");
    public static readonly CodeType WeekDate = new("WeekDate", "Qowaiv");
    public static readonly CodeType Year = new("Year", "Qowaiv");
    public static readonly CodeType YesNo = new("YesNo", "Qowaiv");

    public static readonly CodeType Amount = new("Amount", "Qowaiv.Financial");
    public static readonly CodeType BusinessIdentifierCode = new("BusinessIdentifierCode", "Qowaiv.Financial");
    public static readonly CodeType Currency = new("Currency", "Qowaiv.Financial");
    public static readonly CodeType InternationalBankAccountNumber = new("InternationalBankAccountNumber", "Qowaiv.Financial");
    public static readonly CodeType Money = new("Money", "Qowaiv.Financial");

    public static readonly CodeType Country = new("Country", "Qowaiv.Globalization");

    public static readonly CodeType StreamSize = new("StreamSize", "Qowaiv.IO");

    public static readonly CodeType Fraction = new("Fraction", "Qowaiv.Mathematics");

    public static readonly CodeType Elo = new("StreamSize", "Qowaiv.Statistics");

    public string? Alias { get; init; }
    public bool IsArray { get; init; }
    public bool IsEnum { get; init; }

    public string Display(IEnumerable<Namespace> globals)
    {
        if (Alias is { }) return $"{Alias}{Array()}";
        else if (Namespace.IsEmpty() || globals.Contains(Namespace)) return $"{Name}{Array()}";
        else return $"{Namespace}.{Name}{Array()}";

        string Array() => IsArray ? "[]" : "";
    }

    public override string ToString() 
        => $"{Alias ?? $"{Namespace}.{Name}"}{(IsArray ? "[]" : "")}";

    public CodeType Array() => this with { IsArray = true };

    public CodeType NotArray() => this with { IsArray = false };

    public CodeType Enum() => this with { IsEnum = true };
}

