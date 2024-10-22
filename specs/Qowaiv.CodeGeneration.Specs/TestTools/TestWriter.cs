using System.IO;

namespace Specs.TestTools;

internal static class TestWriter
{
    public static string Write(Code code)
    {
        using var stringWriter = new StringWriter();
        var writer = new CSharpWriter(stringWriter);
        writer.Write(code);
        return stringWriter.ToString();
    }
}
