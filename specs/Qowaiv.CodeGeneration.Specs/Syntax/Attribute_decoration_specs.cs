using Qowaiv.CodeGeneration.Syntax;
using System.Text.Json.Serialization;

namespace Syntax.Attribute_decoration_specs;

public class Supports
{
    [Test]
    public void argumentless()
        => new AttributeInfo(typeof(TestAttribute))
       .Should().HaveContent("[NUnit.Framework.Test]\r\n");

    [Test]
    public void ctor_arguments()
        => new AttributeInfo(typeof(TestAttribute), [typeof(int)])
       .Should().HaveContent("[NUnit.Framework.Test(typeof(int))]\r\n");

    [Test]
    public void parameter_sets()
        => new AttributeInfo(typeof(TestAttribute),
            null, KeyValuePair.Create("Author", (object?)"Qowaiv"))
       .Should().HaveContent("[NUnit.Framework.Test(Author = \"Qowaiv\")]\r\n");

    [Test]
    public void Enum_values()
        => AttributeInfo.System_Text_Json_Serialization_JsonIgnoreAttribute(JsonIgnoreCondition.WhenWritingDefault)
        .Should().HaveContent("[System.Text.Json.Serialization.JsonIgnore(Condition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingDefault)]\r\n");
}

public class Equaly
{
    [Test]
    public void by_value()
    {
        var attr = new AttributeInfo(typeof(TestAttribute),
            null, KeyValuePair.Create("Author", (object?)"Qowaiv"));

        var ottr = new AttributeInfo(typeof(TestAttribute),
            null, KeyValuePair.Create("Author", (object?)"Qowaiv"));

        attr.Equals(ottr).Should().BeTrue();
    }
}
