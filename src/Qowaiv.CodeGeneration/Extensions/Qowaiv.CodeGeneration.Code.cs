﻿using System.IO;

namespace Qowaiv.CodeGeneration;

public static class CodeExtensions
{
    [Pure]
    public static string Stringify(this Code code)
    {
        Guard.NotNull(code, nameof(code));
        using var stream = new MemoryStream();
        var writer = new CSharpWriter(new StreamWriter(stream));
        code.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }
}
