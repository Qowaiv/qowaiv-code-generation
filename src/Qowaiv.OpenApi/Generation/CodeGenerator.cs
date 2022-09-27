using Qowaiv.OpenApi.Decoration;
using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Generation;

public partial class CodeGenerator
{
    public CodeGenerator(
        CodeGeneratorSettings? settings,
        params CodeDecorator[]? decorators)
    {
        Settings = settings ?? new();
        Decorators = decorators?.ToArray() ?? Array.Empty<CodeDecorator>();
    }

    public CodeGeneratorSettings Settings { get; }
    public IReadOnlyCollection<CodeDecorator> Decorators { get; }

    public void Write(Code code, CSharpWriter writer)
    {
        switch (code)
        {
            case CodeModel model: Declaration(model, writer); break;
            case CodeEnum @enum: Declaration(@enum, writer); break;
            default: throw new NotSupportedException();
        }
    }
}
