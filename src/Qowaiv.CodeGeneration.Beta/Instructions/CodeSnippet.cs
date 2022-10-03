using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.CodeGeneration.Instructions;

public sealed class CodeSnippet : Code
{
    private readonly string Text;

    public CodeSnippet(string text)
    {
        Text = Guard.NotNullOrEmpty(text, nameof(text));
    }

    public void WriteTo(CSharpWriter writer)=> writer.Write(Text);
}
