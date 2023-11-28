namespace Qowaiv.SingleValueObjects;

[Flags]
public enum SvoFeatures
{
    Field = /*...............*/ 0x00001,
    GetHashCode = /*.........*/ 0x00002,
    IsEmpty = /*.............*/ 0x00004,
    IsUnknown = /*...........*/ 0x00008,
    IEquatable = /*..........*/ 0x00010,
    EqualsSvo =  /*..........*/ 0x00020,
    ISerializable = /*.......*/ 0x00080,
    IXmlSerializable = /*....*/ 0x00100,
    IJsonSerializable = /*...*/ 0x00200,
    IFormattable = /*........*/ 0x00400,
    IComparable = /*.........*/ 0x00800,
    ComparisonOperators = /*.*/ 0x01000,
    Parsing = /*.............*/ 0x02000,
    Validation  = /*.........*/ 0x04000,
    Utf8 = /*................*/ 0x08000,
    All = /*.................*/ 0x0FFFF,
    Structure = Field | IsEmpty | IsUnknown,
    EqualsSvoAndGetHashCode = EqualsSvo | GetHashCode,
    IsEmptyOrUnknown = IsEmpty | IsUnknown,
    Default = All ^ ComparisonOperators ^ Utf8,
    Continuous = All ^ IsEmpty ^ IsUnknown ^ Utf8,
}
