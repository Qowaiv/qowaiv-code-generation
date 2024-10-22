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
        params object[] becauseArgs)
    {
        using var stream = new MemoryStream();
        var writer = new CSharpWriter(new StreamWriter(stream), settings);
        Subject.WriteTo(writer);
        writer.Flush();

        var csharp = Encoding.UTF8.GetString(stream.ToArray());
        settings ??= new();
        csharp.Should().Match(Normalize(content, settings), because, becauseArgs);

        return new(this);

        static string Normalize(string content, CSharpWriterSettings settings)
        {
            var lines = content.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            content = string.Join(settings.NewLine, lines);
            return content;
        }
    }
}


