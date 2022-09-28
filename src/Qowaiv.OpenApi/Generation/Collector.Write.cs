using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.IO;
using System.IO;

namespace Qowaiv.OpenApi.Generation;

public static class CollectorWriter
{
    public static void WriteTo(this IEnumerable<TypeInfo> types, DirectoryInfo rootDirectory, CSharpWriterSettings? settings = null)
    {
        DeleteExistingGeneratedFiles(rootDirectory);

        foreach (var code in types)
        {
            var path = Path.Combine(rootDirectory.FullName, code.FullName + ".generated.cs");

            var location = new FileInfo(path);
            if (!location.Directory!.Exists)
            {
                location.Directory.Create();
            }
            using var textWriter = new StreamWriter(location.FullName, CSharpWriter.Encoding, new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create });

            code.Write(new CSharpWriter(textWriter, settings));
        }
    }

    private static void DeleteExistingGeneratedFiles(DirectoryInfo rootDirectory)
    {
        var toDelete = rootDirectory.GetFiles("*.generated.cs", SearchOption.AllDirectories);
        foreach (var file in toDelete)
        {
            try
            {
                file.Delete();
            }
            catch (Exception x)
            {
                Console.WriteLine($"Could not delete '{file}': {x.Message}");
            }
        }

        var emptyDirectories = rootDirectory
            .EnumerateDirectories("*", SearchOption.AllDirectories)
            .Where(IsEmpty)
            .ToArray();

        foreach (var directory in emptyDirectories)
        {
            try
            {
                directory.Refresh();
                if (directory.Exists)
                {
                    directory.Delete(true);
                }
            }
            catch (Exception x)
            {
                Console.WriteLine($"Could not delete '{directory}': {x.Message}");
            }
        }

        static bool IsEmpty(DirectoryInfo dir) => !dir.EnumerateFiles("*", SearchOption.AllDirectories).Any();
    }
}
