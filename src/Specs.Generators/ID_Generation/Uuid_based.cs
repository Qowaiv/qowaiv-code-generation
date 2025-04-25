using Qowaiv;
using Qowaiv.OpenApi;
using Qowaiv.TestTools.Globalization;
using System.ComponentModel;

namespace Specs.ID_Generation.Uuid_based;

public class With_domain_logic
{
    [TestCase(true, "33ef5805-c472-4b1f-88bb-2f0723c43889")]
    [TestCase(false, "")]
    public void HasValue_is(bool result, CustomUuid svo) => svo.HasValue.Should().Be(result);

    [Test]
    public void Next_returns() => CustomUuid.Next().Should().Be(CustomUuid.Parse("12345678-1234-1234-1234-123456789abc"));
}

public class Is_comparable
{
    [Test]
    public void to_null_is_1() => Svo.CustomUuid.CompareTo((object?)null).Should().Be(1);

    [Test]
    public void to_CustomUuid_as_object()
    {
        object obj = Svo.CustomUuid;
        Svo.CustomUuid.CompareTo(obj).Should().Be(0);
    }

    [Test]
    public void to_CustomUuid_only()
        => new object().Invoking(Svo.CustomUuid.CompareTo).Should().Throw<ArgumentException>();

    [Test]
    public void can_be_sorted_using_compare()
    {
        var sorted = new[]
        {
            CustomUuid.Empty,
            CustomUuid.Parse("33ef5805-c472-4b1f-88bb-2f0723c43889"),
            CustomUuid.Parse("58617a65-2a14-4a9a-82a8-c1a82c956c25"),
            CustomUuid.Parse("853634b4-e474-4b0f-b9ba-01fc732b56d8"),
            CustomUuid.Parse("93ca7b43-8fb3-44e5-a21f-feeebb8e0f6f"),
            CustomUuid.Parse("f5e6c39a-adcf-4eca-bcf2-6b8317ac502c"),
        };

        var list = new List<CustomUuid> { sorted[3], sorted[4], sorted[5], sorted[2], sorted[0], sorted[1] };
        list.Sort();
        list.Should().BeEquivalentTo(sorted);
    }
}

public class Supports_type_conversion
{
    [Test]
    public void via_TypeConverter_registered_with_attribute()
        => typeof(CustomUuid).Should().BeDecoratedWith<TypeConverterAttribute>();

    [Test]
    public void from_null_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.FromNull<string>().To<CustomUuid>().Should().Be(CustomUuid.Empty);
        }
    }

    [Test]
    public void from_empty_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From(string.Empty).To<CustomUuid>().Should().Be(CustomUuid.Empty);
        }
    }

    [Test]
    public void from_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.From("8A1A8C42-D2FF-E254-E26E-B6ABCBF19420").To<CustomUuid>().Should().Be(Svo.CustomUuid);
        }
    }

    [Test]
    public void to_string()
    {
        using (TestCultures.en_GB.Scoped())
        {
            Converting.ToString().From(Svo.CustomUuid).Should().Be("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");
        }
    }

    [Test]
    public void from_Guid()
        => Converting.From(Svo.Guid).To<CustomUuid>().Should().Be(Svo.CustomUuid);


    [Test]
    public void from_Uuid()
        => Converting.From(Svo.Uuid).To<CustomUuid>().Should().Be(Svo.CustomUuid);

    [Test]
    public void to_Guid()
        => Converting.To<Guid>().From(Svo.CustomUuid).Should().Be(Svo.Guid);

    [Test]
    public void to_Uuid()
        => Converting.To<Uuid>().From(Svo.CustomUuid).Should().Be(Svo.Uuid);
}

public class Supports_JSON_serialization
{
    [Test]
    public void writes_null_for_default_value()
        => JsonTester.Write(default(CustomUuid)).Should().BeNull();

    [Test]
    public void writes_GUID_for_non_default_value()
        => JsonTester.Write(Svo.CustomUuid).Should().Be("8a1a8c42-d2ff-e254-e26e-b6abcbf19420");

#if NET6_0_OR_GREATER

    [Test]
    public void System_Text_JSON_deserialization_of_dictionary_keys()
    {
        System.Text.Json.JsonSerializer.Deserialize<Dictionary<CustomUuid, int>>(@"{""8a1a8c42-d2ff-e254-e26e-b6abcbf19420"":42}")
            .Should().BeEquivalentTo(new Dictionary<CustomUuid, int>()
            {
                [Svo.CustomUuid] = 42,
            });
    }

    [Test]
    public void System_Text_JSON_serialization_of_dictionary_keys()
    {
        var dictionary = new Dictionary<CustomUuid, int>()
        {
            [default] = 17,
            [Svo.CustomUuid] = 42,
        };
        System.Text.Json.JsonSerializer.Serialize(dictionary)
            .Should().Be(@"{"""":17,""8a1a8c42-d2ff-e254-e26e-b6abcbf19420"":42}");
    }
#endif
}

public class Is_Open_API_data_type
{
    [Test]
    public void with_info()
        => OpenApiDataType.FromType(typeof(CustomUuid))
        .Should().Be(new OpenApiDataType(
            dataType: typeof(CustomUuid),
            description: "UUID based identifier",
            example: "Qowaiv_SVOLibrary_GUIA",
            type: "string",
            format: "guid"));
}
