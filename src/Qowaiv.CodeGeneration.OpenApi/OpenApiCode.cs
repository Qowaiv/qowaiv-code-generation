﻿using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Qowaiv.CodeGeneration.IO;
using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Validation.Abstractions;
using System.IO;

namespace Qowaiv.CodeGeneration.OpenApi;

[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public sealed class OpenApiCode : IReadOnlyCollection<Code>
{
    public static readonly OpenApiCode Empty = new(Array.Empty<Code>());

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyCollection<Code> Code;

    private OpenApiCode(IReadOnlyCollection<Code> code) => Code = code;

    /// <inheritdoc />
    public int Count => Code.Count;

    /// <summary>Only keep code that the matches the predicate or is used by it.</summary>
    [Pure]
    public OpenApiCode Filter(Predicate<TypeBase> predicate) 
        => new(Code
            .OfType<TypeBase>()
            .Where(t => predicate(t))
            .IncludeUsings()
            .Cast<Code>()
            .ToArray());

    public void Save(CodeFileWriterSettings codeFileSettings, CSharpWriterSettings? csharpSettings = null)
    {
        Guard.NotNull(codeFileSettings).RootDirectory.Ensure();
        csharpSettings ??= new();

        DeleteExisting(codeFileSettings);

        foreach (var code in this.OfType<TypeBase>())
        {
            Write(code, codeFileSettings, csharpSettings);
        }
    }

    private static void Write(TypeBase codeFile, CodeFileWriterSettings codeFileSettings, CSharpWriterSettings csharpSettings)
    {
        var prefix = codeFileSettings.RootNamespace.ToString();
        var fileName = $"{codeFile.TypeName.Namespace}.{codeFile.Name}{codeFileSettings.FileNameSuffix}.cs";

        if (fileName.StartsWith(prefix))
        {
            fileName = fileName[prefix.Length..].TrimStart('.');
        }

        var file = new FileInfo(Path.Combine(codeFileSettings.RootDirectory.FullName, fileName));
        
        file.Directory.Ensure();

        using var textWriter = new StreamWriter(file.FullName, new FileStreamOptions
        {
            Access = FileAccess.Write,
            Mode = FileMode.CreateNew,
        });

        var writer = new CSharpWriter(textWriter, csharpSettings);

        foreach(var header in codeFileSettings.Headers)
        {
            writer.Write(header);
        }
        writer.Write((Code)codeFile);
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

    /// <inheritdoc />
    public IEnumerator<Code> GetEnumerator()=> Code.GetEnumerator();

    /// <inheritdoc />
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentLocation">
    /// The file location representing an <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(FileInfo documentLocation, OpenApiTypeResolver resolver)
    {
        using var stream = Guard.Exists(documentLocation).OpenRead();
        return Resolve(stream, resolver);
    }

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentStream">
    /// The stream representing an <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(Stream documentStream, OpenApiTypeResolver resolver)
    {
        var reader = new OpenApiStreamReader();
        var document = reader.Read(documentStream, out var diagnostic);

        return Result.For(Empty, Messages(diagnostic))
            .Act(_ => Result.For(Resolve(document, resolver)));

        IEnumerable<IValidationMessage> Messages(OpenApiDiagnostic diagnostic)
            => diagnostic.Errors
                .Select(e => ValidationMessage.Error(e.Message, e.Pointer))
                .Concat(diagnostic.Warnings
                    .Where(e => e.Message != "Data and type mismatch found.")
                    .Select(e => ValidationMessage.Warn(e.Message, e.Pointer)));
    }

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="document">
    /// The document to walk through.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static OpenApiCode Resolve(OpenApiDocument document, OpenApiTypeResolver resolver)
    {
        Guard.NotNull(document);
        Guard.NotNull(resolver);

        return new OpenApiCode(resolver.Walk(document)).Filter(_ => true);
    }
}