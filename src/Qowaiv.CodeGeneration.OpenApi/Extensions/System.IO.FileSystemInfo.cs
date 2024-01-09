using Qowaiv.Diagnostics.Contracts;

namespace System.IO;

internal static class QowaivFyleSystemInfoExtensions
{
    [FluentSyntax]
    public static DirectoryInfo Ensure(this DirectoryInfo? dir)
    {
        dir ??= new(".");
        if (!dir.Exists)
        {
            dir.Create();
        }
        return dir;
    }
}
