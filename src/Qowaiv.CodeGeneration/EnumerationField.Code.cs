using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public partial class EnumerationField : Code
{
    /// <inheritdoc />
    public virtual void Write(CSharpWriter writer)
    {
        writer.Indent().Line(Name);
    }
}
