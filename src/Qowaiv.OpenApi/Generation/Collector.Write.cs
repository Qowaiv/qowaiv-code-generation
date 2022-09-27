using Qowaiv.OpenApi.Collection;
using Qowaiv.OpenApi.Decoration;
using Qowaiv.OpenApi.IO;
using System.IO;

namespace Qowaiv.OpenApi.Generation;

public static class CollectorWriter
{
    public static void Write(
        this Collector collector,
        CodeGeneratorSettings? settings = null,
        CSharpWriterSettings? writerSettings = null,
        params CodeDecorator[] decorators)
    {
        settings ??= new();
        writerSettings ??= new();

        DeleteExistingGeneratedFiles(settings);

        var defaultNamespace = collector.Resolver.DefaultNamespace.Name ?? string.Empty;

        foreach (var code in collector)
        {
            var ns = code.Type.Namespace.Name;

            if (ns == defaultNamespace || ns.StartsWith($"{defaultNamespace}."))
            {
                ns = ns[defaultNamespace.Length..].TrimStart('.').Replace('.', '\\');
            }

            var path = Path.Combine(settings.RootLocation.FullName, ns, code.Type.Name + ".generated.cs");

            var location = new FileInfo(path);
            if (!location.Directory!.Exists)
            {
                location.Directory.Create();
            }
            using var textWriter = new StreamWriter(location.FullName, CSharpWriter.Encoding, new FileStreamOptions { Access = FileAccess.Write, Mode = FileMode.Create });

            var generator = new CodeGenerator(settings, decorators);

            generator.Write(code, new CSharpWriter(writerSettings, textWriter));
        }
    }

    private static void DeleteExistingGeneratedFiles(CodeGeneratorSettings settings)
    {
        if (!settings.DeleteExistingGeneratedFiles || !settings.RootLocation.Exists) return;

        var toDelete = settings.RootLocation.GetFiles("*.generated.cs", SearchOption.AllDirectories);
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

        var emptyDirectories = settings.RootLocation
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
