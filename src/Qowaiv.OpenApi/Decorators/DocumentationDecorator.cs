using Microsoft.OpenApi.Any;
using System.Text;

namespace Qowaiv.OpenApi.Decoration;

public class DocumentationDecorator : CodeDecorator
{
    public override void Declaration(CSharpWriter writer, CodeModel model)
    {
        writer.Documentation("summary", model.Schema.Description);
    }
    public override void Constructor(CSharpWriter writer, CodeModel model)
    {
        writer.Documentation("summary", $@"Creates a new instance of the <see cref=""{model.Type.Name}"" /> class.", literal: true);
    }

    public override void Property(CSharpWriter writer, CodeProperty property)
    {
        writer.Documentation("summary", property.Schema.Description);

        var remarks = new StringBuilder($"Type: {property.Schema.Type}");

        if (!string.IsNullOrEmpty(property.Schema.Format))
        {
            remarks.Append($", Format: {property.Schema.Format}");
        }
        if (property.Schema.Pattern is { })
        {
            remarks.Append($", Pattern: {property.Schema.Pattern}");
        }
        if (property.Schema.Example is OpenApiString str)
        {
            remarks.Append($", Example: {str.Value}");
        }
        remarks.Append('.');

        writer.Documentation("remarks", remarks.ToString());
    }


    public override void Declaration(CSharpWriter writer, CodeEnum @enum)
    {
        writer.Documentation("summary", @enum.Schema.Description);
    }

    public override void NameValue(CSharpWriter writer, CodeNameValue nameValue)
    {
        writer.Documentation("summary", nameValue.Name);
    }
}
