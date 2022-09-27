using Qowaiv.Financial;
using Qowaiv.Globalization;
using Qowaiv.IO;
using Qowaiv.Mathematics;
using Qowaiv.Statistics;

namespace Qowaiv.OpenApi;

public partial class TypeInfo
{
    public static readonly Type Object = typeof(object);

    public static readonly Type String = typeof(string);
    public static readonly Type Boolean = typeof(bool);

    public static readonly Type Decimal = typeof(decimal);
    public static readonly Type Int32 = typeof(int);
    public static readonly Type Int64 = typeof(int);

    public static readonly Type Guid = typeof(Guid);
    public static readonly Type DateOnly = typeof(DateOnly);
    public static readonly Type DateTime = typeof(DateTime);

    public static readonly Type Date = typeof(Date);
    public static readonly Type DateSpan = typeof(DateSpan);
    public static readonly Type EmailAddress = typeof(EmailAddress);
    public static readonly Type EmailAddressCollection = typeof(EmailAddressCollection);
    public static readonly Type HouseNumber = typeof(HouseNumber);
    public static readonly Type LocalDateTime = typeof(LocalDateTime);
    public static readonly Type Month = typeof(Month);
    public static readonly Type MonthSpan = typeof(MonthSpan);
    public static readonly Type Percentage = typeof(Percentage);
    public static readonly Type PostalCode = typeof(PostalCode);
    public static readonly Type Sex = typeof(Sex);
    public static readonly Type Uuid = typeof(Uuid);
    public static readonly Type WeekDate = typeof(WeekDate);
    public static readonly Type Year = typeof(Year);
    public static readonly Type YesNo = typeof(YesNo);

    public static readonly Type Amount = typeof(Amount);
    public static readonly Type BusinessIdentifierCode = typeof(BusinessIdentifierCode);
    public static readonly Type Currency = typeof(Currency);
    public static readonly Type InternationalBankAccountNumber = typeof(InternationalBankAccountNumber);
    public static readonly Type Money = typeof(Money);
    public static readonly Type Country = typeof(Country);
    public static readonly Type StreamSize = typeof(StreamSize);
    public static readonly Type Fraction = typeof(Fraction);
    public static readonly Type Elo = typeof(Elo);
}
