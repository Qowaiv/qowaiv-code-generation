using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Decoration;

public class CodeDecorator
{
    public virtual IEnumerable<CodeType> Interfaces => Array.Empty<CodeType>();

    public virtual void Declaration(CSharpWriter writer, CodeEnum @enuml) { }

    public virtual void NameValue(CSharpWriter writer, CodeNameValue nameValue) { }

    public virtual void Declaration(CSharpWriter writer, CodeModel model) { }

    public virtual void Constructor(CSharpWriter writer, CodeModel model) { }

    public virtual void Property(CSharpWriter writer, CodeProperty property) { }
}
