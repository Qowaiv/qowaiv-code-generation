using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public partial class EnumerationField : Code
{
    /// <inheritdoc />
    public virtual void WriteTo(CSharpWriter writer)
    {
        writer.Indent().Write(Name);
    }
}
