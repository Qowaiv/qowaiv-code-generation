using System.Text.Json;
using System.Text.Json.Serialization;

namespace Open_API.JSON_serialization_specs;

public class Serializes
{
    [Test]
    public void Model_with_derived_types()
    {
        var json = @"{""Items"":[{""Value1"":""1.34534"",""Type"":""Derived1""},{""Value2"":""2.85464"",""Type"":""Derived2""}]}";

        var model = JsonSerializer.Deserialize<MyContainer>(json);

        model.Should().BeEquivalentTo(new MyContainer
        {
            Items =
            [
                new Derived1{ Type = "Derived1", Value1 = "1.34534" },
                new Derived2{ Type = "Derived2", Value2 = "2.85464" },
            ]
        });
    }
}

public class Deserializes
{
    [Test]
    public void Model_with_derived_types()
    {
        var model = new MyContainer
        {
            Items =
            [
                new Derived1{ Type = "Derived1", Value1 = "1.34534" },
                new Derived2{ Type = "Derived2", Value2 = "2.85464" },
            ]
        };

        var json = JsonSerializer.Serialize(model);


        Console.WriteLine(json);
    }
}


internal sealed record MyContainer
{
    public MyBase[] Items { get; init; } = [];
}

[JsonDerivedType(typeof(Derived1))]
[JsonDerivedType(typeof(Derived2))]
internal record MyBase
{
    public string? Type { get; init; }
}
internal sealed record Derived1 : MyBase
{
    public string? Value1 { get; init; }
}

internal sealed record Derived2 : MyBase
{
    public string? Value2 { get; init; }
}
