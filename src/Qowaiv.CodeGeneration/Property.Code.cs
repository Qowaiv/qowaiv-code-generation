using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration;

public partial class Property : Code
{
    /// <inheritdoc />
    public virtual void Write(CSharpWriter writer)
    {
        foreach (var decoration in Decorations) writer.Write(decoration);

        writer.Indent()
            .Write("public ").Write(PropertyType).Write(' ').Write(Name).Write(' ').Write(PropertyAccess.Code())
            .Line();
    }
}
