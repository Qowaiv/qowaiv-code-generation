﻿using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Generation;

public partial class CodeGenerator
{
    private void Declaration(CodeEnum @enum, CSharpWriter writer)
    {
        writer
            .AutoGeneratedHeader()
            .NamespaceDeclration(@enum.Type.Namespace);

        foreach (var decorator in Decorators) decorator.Declaration(writer, @enum);
        using (writer.Line($"public enum {@enum.Type.Name}").CodeBlock())
        {
            writer.Write(
                writes: @enum.Select(WriteNameValue),
                split: writer => writer.Line());
        }

        Action<CSharpWriter> WriteNameValue(CodeNameValue nameValue) => writer => NameValue(nameValue, writer);
    }

    private void NameValue(CodeNameValue nameValue, CSharpWriter writer)
    {
        foreach (var decorator in Decorators) decorator.NameValue(writer, nameValue);
        writer.Indent().Line($"{nameValue.Name},");
    }
}