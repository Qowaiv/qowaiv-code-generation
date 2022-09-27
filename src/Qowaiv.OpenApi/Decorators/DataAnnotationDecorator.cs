using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Decoration;

public class DataAnnotationDecorator : CodeDecorator
{
    public static readonly CodeType Range = new("RangeAttribute", "System.ComponentModel.DataAnnotations");
    public static readonly CodeType AllowedValues = new("AllowedValuesAttribute", "System.ComponentModel.DataAnnotations");
    public static readonly CodeType RegularExpression = new("RegularExpressionAttribute", "System.ComponentModel.DataAnnotations");
    public static readonly CodeType Required = new("RequiredAttribute", "System.ComponentModel.DataAnnotations");
    public static readonly CodeType Any = new("AnyAttribute", "Qowaiv.Validation.DataAnnotations");
    public static readonly CodeType Optional = new("OptionalAttribute", "Qowaiv.Validation.DataAnnotations");

    public override void Property(CSharpWriter writer, CodeProperty property)
    {
        PropertyNullable(writer, property);
        PropertyPattern(writer, property);
        
    }
    protected virtual void PropertyNullable(CSharpWriter writer, CodeProperty property)
    {
        if (!property.Schema.Nullable)
        {
            writer.Indent()
                .Write('[')
                .Write(property.Type.IsArray ? Any : Required, attribute: true)
                .Line(']');
        }
        else
        {
            writer.Indent()
                .Write('[')
                .Write(Optional, attribute: true)
                .Line(']');
        }
    }

    protected virtual void PropertyPattern(CSharpWriter writer, CodeProperty property)
    {
        if (property.Type == CodeType.String && property.Schema.Pattern is { })
        {
            writer.Indent()
                .Write('[')
                .Write(RegularExpression, attribute: true)
                .Write('(')
                .Literal(property.Schema.Pattern.ToString())
                .Line($")]");
        }
    }
}

