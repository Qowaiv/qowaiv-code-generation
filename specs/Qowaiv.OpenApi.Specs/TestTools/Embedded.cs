using System.IO;

namespace Qowaiv.OpenApi.Specs.TestTools;

public sealed class Embedded
{
    private Embedded() { }

    public static Stream Stream(string path)
        => typeof(Embedded).Assembly.GetManifestResourceStream($"Qowaiv.OpenApi.Specs.{path}") 
        ?? throw new FileNotFoundException("Embedded resource does not exist.", $"Qowaiv.OpenApi.Specs.{path}");

    public static string String(string path)
    {
        using var reader = new StreamReader(Stream(path));
        return reader.ReadToEnd();
    }
}

