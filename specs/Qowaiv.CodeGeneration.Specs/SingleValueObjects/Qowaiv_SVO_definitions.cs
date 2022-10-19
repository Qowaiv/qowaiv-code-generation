﻿using Qowaiv.IO;
using Qowaiv;
using System.IO;
using Qowaiv.SingleValueObjects;

namespace Qowaiv_SVO_definitions;

public class Generation_of
{
    private static string Root = @"c:\code\Qowaiv";

    [TestCase("Date", typeof(DateTime), "date", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field)]
    [TestCase("DateSpan", typeof(ulong), "date span", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.EqualsSvo)]
    [TestCase("EmailAddress", typeof(string), "email address", "Qowaiv", SvoFeatures.Default)]
    [TestCase("Gender", typeof(byte), "gender", "Qowaiv", SvoFeatures.Default)]
    [TestCase("HouseNumber", typeof(int), "house number", "Qowaiv", SvoFeatures.Default | SvoFeatures.ComparisonOperators)]
    [TestCase("LocalDateTime", typeof(DateTime), "local date time", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field)]
    [TestCase("Month", typeof(byte), "month", "Qowaiv", SvoFeatures.Default)]
    [TestCase("MonthSpan", typeof(int), "month span", "Qowaiv", SvoFeatures.Continuous)]
    [TestCase("Percentage", typeof(decimal), "percentage", "Qowaiv", SvoFeatures.Continuous)]
    [TestCase("PostalCode", typeof(string), "postal code", "Qowaiv", SvoFeatures.Default)]
    [TestCase("Sex", typeof(byte), "sex", "Qowaiv", SvoFeatures.Default)]
    [TestCase("Uuid", typeof(Guid), "UUID", "Qowaiv", SvoFeatures.DefaultExcludingCulture ^ SvoFeatures.IsUnknown)]
    [TestCase("WeekDate", typeof(Date), "week date", "Qowaiv", SvoFeatures.Continuous ^ SvoFeatures.Field ^ SvoFeatures.ISerializable)]
    [TestCase("Year", typeof(short), "year", "Qowaiv", SvoFeatures.Default)]
    [TestCase("YesNo", typeof(byte), "yes-no", "Qowaiv", SvoFeatures.Default)]

    [TestCase("Amount", typeof(decimal), "amount", "Qowaiv.Financial", SvoFeatures.Continuous, "QowaivMessages.FormatExceptionFinancialAmount")]
    [TestCase("BusinessIdentifierCode", typeof(string), "BIC", "Qowaiv.Financial", SvoFeatures.Default)]
    [TestCase("Currency", typeof(string), "currency", "Qowaiv.Financial", SvoFeatures.Default)]
    [TestCase("InternationalBankAccountNumber", typeof(string), "IBAN", "Qowaiv.Financial", SvoFeatures.Default)]
    [TestCase("Money", typeof(decimal), "money", "Qowaiv.Financial", (SvoFeatures.Continuous)
        ^ SvoFeatures.ISerializable
        ^ SvoFeatures.EqualsSvo
        ^ SvoFeatures.Field
        ^ SvoFeatures.GetHashCode)]

    [TestCase("CasRegistryNumber", typeof(long), "CAS Registry Number", "Qowaiv.Chemistry", SvoFeatures.Default)]

    [TestCase("Country", typeof(string), "country", "Qowaiv.Globalization", SvoFeatures.Default)]

    [TestCase("StreamSize", typeof(long), "stream size", "Qowaiv.IO", SvoFeatures.Continuous ^ SvoFeatures.Field ^ SvoFeatures.IFormattable)]

    [TestCase("Elo", typeof(double), "elo", "Qowaiv.Statistics", SvoFeatures.Continuous)]

    [TestCase("Fraction", typeof(long), "fraction", "Qowaiv.Mathematics", (SvoFeatures.Continuous)
        ^ SvoFeatures.ISerializable
        ^ SvoFeatures.EqualsSvo
        ^ SvoFeatures.Field)]

    [TestCase("InternetMediaType", typeof(string), "Internet media type", "Qowaiv.Web", SvoFeatures.DefaultExcludingCulture)]
    public void Qowaiv(string name, Type underlying, string fulleName, string ns, SvoFeatures features, string formatExceptionMessage = null)
    {
        var sub = ns.Replace("Qowaiv", "").Replace(".", @"\");
        var path = $@"{Root}\src\Qowaiv\Generated\{sub}\{name}.generated.cs".Replace(@"\\", @"\");
        var init = $@"{Root}\src\Qowaiv\{sub}\{name}.cs".Replace(@"\\", @"\");
        var json = $@"{Root}\src\Qowaiv\Json\{sub}\{name}JsonConverter.cs".Replace(@"\\", @"\");
        var spec = $@"{Root}\specs\Qowaiv.Specs\{sub}\{name}_specs.cs".Replace(@"\\", @"\");
        Generate(name, underlying, fulleName, ns, features, path, init, json, spec, formatExceptionMessage);
    }

    [TestCase("Timestamp", typeof(ulong), "timestamp", "Qowaiv.Sql", SvoFeatures.Continuous)]
    public void Qowaiv_DataClient(string name, Type underlying, string fulleName, string ns, SvoFeatures features, string formatExceptionMessage = null)
    {
        var path = $@"{Root}\src\Qowaiv.Data.SqlClient\Generated\{name}.generated.cs";
        var init = $@"{Root}\src\Qowaiv.Data.SqlClient\Sql\{name}.cs";
        var json = $@"{Root}\src\Qowaiv.Data.SqlClient\Json\Sql\{name}JsonConverter.cs";
        var spec = $@"{Root}\specs\Qowaiv.Specs\Sql\{name}_specs.cs".Replace(@"\\", @"\");
        Generate(name, underlying, fulleName, ns, features, path, init, json, spec, formatExceptionMessage);
    }

    private static void Generate(
        string name,
        Type underlying,
        string fulleName,
        string ns,
        SvoFeatures features,
        string path,
        string init,
        string json,
        string spec,
        string formatExceptionMessage)
    {
        var arguments = new SvoArguments
        {
            Name = name,
            Underlying = underlying,
            FullName = fulleName,
            Namespace = ns,
            Features = features,
            FormatExceptionMessage = formatExceptionMessage ?? $"QowaivMessages.FormatException{name}"
        };

        Generate(new FileInfo(path), arguments);
        GenerateInitial(new FileInfo(init), arguments);
        GenerateJsonConverter(new FileInfo(json), arguments);
        GenerateSpecs(new FileInfo(spec), arguments);

        static void Generate(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.GeneratedCode, @override: true);

        static void GenerateInitial(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.Initial);

        static void GenerateSpecs(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.Specs);

        static void GenerateJsonConverter(FileInfo location, SvoArguments arguments)
            => Write(location, arguments, SvoTemplate.JsonConverter);

        static void Write(
            FileInfo location, 
            SvoArguments arguments,
            SvoTemplate template,
            bool @override = false)
        {
            if (@override || !location.Exists || location.Length < StreamSize.KB)
            {
                if (!location.Directory!.Exists)
                {
                    location.Directory.Create();
                }
                var writer = new CSharpWriter(new StreamWriter(location.FullName, false, Encoding.UTF8));
                writer.Write(template.Transform(arguments));
                writer.Flush();
            }
        }
    }
}
