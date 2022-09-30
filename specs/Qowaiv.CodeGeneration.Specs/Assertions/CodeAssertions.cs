using Qowaiv.CodeGeneration;
using System.IO;

namespace FluentAssertions;

public class CodeAssertions
{
    public CodeAssertions(Code subject) => Subject = Guard.NotNull(subject, nameof(subject));

    public Code Subject { get; }

    public AndConstraint<CodeAssertions> HaveContent(
        string content,
        CSharpWriterSettings? settings = null, 
        string because = "",
        params object[] becuaseArgs)
    {
        using var stream = new MemoryStream();
        var writer = new CSharpWriter(new StreamWriter(stream), settings);
        Subject.WriteTo(writer);
        writer.Flush();
        
        var csharp = Encoding.UTF8.GetString(stream.ToArray());

        csharp.Should().Be(content, because, becuaseArgs);

        return new(this);
    }
}


