using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Decoration;

public class NewtonsoftJsontJsonDecorator : CodeDecorator
{
    public static readonly CodeType JsonConstructor = new("JsonConstructorAttribute", "Newtonsoft.Json");
    public static readonly CodeType JsonProperty = new("JsonProperty", "Newtonsoft.Json");

    public override void Constructor(CSharpWriter writer, CodeModel model)
        => writer.Indent()
        .Write('[').Write(JsonConstructor, attribute: true).Line(']');

    public override void Property(CSharpWriter writer, CodeProperty property)
        => writer.Indent()
        .Write('[').Write(JsonProperty, attribute: true).Write('(').Literal(property.Schema.Name).Line(")]");

}
