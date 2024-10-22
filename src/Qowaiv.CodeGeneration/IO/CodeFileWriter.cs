using System.IO;

namespace Qowaiv.CodeGeneration.IO;

public static class CodeFileWriter
{
    public static void Save(this IEnumerable<Code> code, CodeFileWriterSettings codeFileSettings, CSharpWriterSettings? csharpSettings = null)
    {
        Guard.NotNull(code);
        Guard.NotNull(codeFileSettings);

        var dir = codeFileSettings.RootDirectory.Ensure();

        csharpSettings ??= new();

        DeleteExisting(codeFileSettings);

        foreach (var codeFile in code.OfType<TypeBase>())
        {
            var fileName = $"{codeFile.TypeName.Namespace}.{codeFile.Name}{codeFileSettings.FileNameSuffix}.cs";
            if (fileName.StartsWith(codeFileSettings.RootNamespace.ToString()))
            {
                fileName = fileName[codeFileSettings.RootNamespace.ToString().Length..].TrimStart('.');
            }
            var file = new FileInfo(Path.Combine(dir.FullName, fileName));
            file.Directory.Ensure();

            using var textWriter = new StreamWriter(file.FullName, new FileStreamOptions
            {
                Access = FileAccess.Write,
                Mode = FileMode.CreateNew,
            });

            var writer = new CSharpWriter(textWriter, csharpSettings);
            foreach (var header in codeFileSettings.Headers)
            {
                writer.Write(header);
            }
            writer.Write((Code)codeFile);
        }
    }

    [FluentSyntax]
    private static DirectoryInfo Ensure(this DirectoryInfo? dir)
    {
        dir ??= new(".");
        if (!dir.Exists)
        {
            dir.Create();
        }
        return dir;
    }

    private static void DeleteExisting(CodeFileWriterSettings codeFileSettings)
    {
        if (codeFileSettings.DeletePattern is { })
        {
            foreach (var file in codeFileSettings.RootDirectory.EnumerateFiles(codeFileSettings.DeletePattern, SearchOption.TopDirectoryOnly))
            {
                file.Delete();
            }
        }
    }
}
