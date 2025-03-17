using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Readers;
using Qowaiv.CodeGeneration.IO;
using Qowaiv.CodeGeneration.Syntax;
using Qowaiv.Validation.Abstractions;
using System.IO;

namespace Qowaiv.CodeGeneration.OpenApi;

/// <summary>A (read-only) collection of Open API code.</summary>
[DebuggerDisplay("Count = {Count}")]
[DebuggerTypeProxy(typeof(Diagnostics.CollectionDebugView))]
public sealed class OpenApiCode : IReadOnlyCollection<Code>
{
    /// <summary>Represents an empty collection.</summary>
    public static readonly OpenApiCode Empty = new([]);

    [DebuggerBrowsable(DebuggerBrowsableState.Never)]
    private readonly IReadOnlyCollection<Code> Code;

    private OpenApiCode(IReadOnlyCollection<Code> code) => Code = code;

    /// <inheritdoc />
    public int Count => Code.Count;

    /// <summary>Only keep code that the matches the predicate or is used by it.</summary>
    [Pure]
    public OpenApiCode Filter(Predicate<TypeBase> predicate)
        => new([.. Code
            .OfType<TypeBase>()
            .Where(t => predicate(t))
            .IncludeUsings()
            .OfType<Code>()]);

    /// <summary>Saves the code.</summary>
    /// <param name="codeFileSettings">
    /// The code file settings to apply.
    /// </param>
    /// <param name="csharpSettings">
    /// The optional C# writer settings.
    /// </param>
    public void Save(CodeFileWriterSettings codeFileSettings, CSharpWriterSettings? csharpSettings = null)
    {
        Guard.NotNull(codeFileSettings).RootDirectory.Ensure();
        GuardTypes();

        csharpSettings ??= new();

        DeleteExisting(codeFileSettings);

        foreach (var code in this.OfType<TypeBase>())
        {
            Write(code, codeFileSettings, csharpSettings);
        }

        void GuardTypes()
        {
            var unique = new HashSet<TypeName>();

            foreach (var type in this.OfType<TypeBase>().Select(t => t.TypeName))
            {
                if (!unique.Add(type)) throw new DuplicateType($"Contains '{type}' multiple times");
            }
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

        foreach (var header in codeFileSettings.Headers)
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
    [Pure]
    public IEnumerator<Code> GetEnumerator() => Code.GetEnumerator();

    /// <inheritdoc />
    [Pure]
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
        => Resolve(documentLocation, resolver, null);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentLocations">
    /// The file locations representing <see cref="OpenApiDocument"/>s.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(IEnumerable<FileInfo> documentLocations, OpenApiTypeResolver resolver)
        => Resolve(documentLocations, resolver, null);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentLocation">
    /// The file location representing an <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="readerOptions">
    /// The options used for reading.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(FileInfo documentLocation, OpenApiTypeResolver resolver, OpenApiReaderOptions? readerOptions)
        => Resolve([Guard.Exists(documentLocation)], resolver, readerOptions);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentLocations">
    /// The file locations representing <see cref="OpenApiDocument"/>s.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="readerOptions">
    /// The options used for reading.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(IEnumerable<FileInfo> documentLocations, OpenApiTypeResolver resolver, OpenApiReaderOptions? readerOptions)
        => Resolve(
            reader =>
            {
                var results = new List<ReadResult>();

                foreach (var documentLocation in documentLocations)
                {
                    using var stream = Guard.Exists(documentLocation).OpenRead();
                    var document = reader.Read(stream, out var diagnostic);
                    results.Add(new(document, diagnostic));
                }
                return results;
            },
            resolver,
            readerOptions);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentStream">
    /// The stream representing an <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(Stream documentStream, OpenApiTypeResolver resolver)
        => Resolve(documentStream, resolver, null);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentStream">
    /// The stream representing an <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="readerOptions">
    /// The options used for reading.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(Stream documentStream, OpenApiTypeResolver resolver, OpenApiReaderOptions? readerOptions)
    {
        readerOptions ??= new();
        var reader = new OpenApiStreamReader();
        var document = reader.Read(documentStream, out var diagnostic);

        return Result.For(Empty, Messages(diagnostic))
            .Act(_ => Result.For(Resolve(document, resolver)));

        IEnumerable<IValidationMessage> Messages(OpenApiDiagnostic diagnostic)
            => diagnostic.Errors
                .Select(e => readerOptions.IgnoreErrors
                           ? ValidationMessage.Warn(e.Message, e.Pointer)
                           : ValidationMessage.Error(e.Message, e.Pointer))
                .Concat(diagnostic.Warnings
                    .Where(e => e.Message != "Data and type mismatch found.")
                    .Select(e => ValidationMessage.Warn(e.Message, e.Pointer)));
    }

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentStreams">
    /// The streams representing <see cref="OpenApiDocument"/>s.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(IEnumerable<Stream> documentStreams, OpenApiTypeResolver resolver)
        => Resolve(documentStreams, resolver, null);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documentStreams">
    /// The streams representing <see cref="OpenApiDocument"/>s.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    /// <param name="readerOptions">
    /// The options used for reading.
    /// </param>
    [Pure]
    public static Result<OpenApiCode> Resolve(IEnumerable<Stream> documentStreams, OpenApiTypeResolver resolver, OpenApiReaderOptions? readerOptions) => Resolve(
            reader =>
            {
                var results = new List<ReadResult>();

                foreach (var documentStream in documentStreams)
                {
                    var document = reader.Read(documentStream, out var diagnostic);
                    results.Add(new(document, diagnostic));
                }
                return results;
            },
            resolver,
            readerOptions);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="document">
    /// The document to walk through.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static OpenApiCode Resolve(OpenApiDocument document, OpenApiTypeResolver resolver)
        => Resolve([Guard.NotNull(document)], resolver);

    /// <summary>Resolves the code defined in the <see cref="OpenApiDocument"/>.</summary>
    /// <param name="documents">
    /// The document to walk through.
    /// </param>
    /// <param name="resolver">
    /// The resolver to interpreter the <see cref="OpenApiDocument"/>.
    /// </param>
    [Pure]
    public static OpenApiCode Resolve(IEnumerable<OpenApiDocument> documents, OpenApiTypeResolver resolver)
        => new OpenApiCode(Guard.NotNull(resolver).Walk(documents)).Filter(_ => true);

    [Pure]
    private static Result<OpenApiCode> Resolve(Func<OpenApiStreamReader, IEnumerable<ReadResult>> read, OpenApiTypeResolver resolver, OpenApiReaderOptions? readerOptions)
    {
        readerOptions ??= new();
        var reader = new OpenApiStreamReader();
        var result = Result.OK;
        var documents = new List<OpenApiDocument>();

        foreach (var (document, diagnostic) in read(reader))
        {
            result = Result.WithMessages(result.Messages.Concat(Messages(diagnostic)));
            documents.Add(document);
        }

        return result.IsValid
            ? Result.For(Resolve(documents, resolver))
            : Result.WithMessages<OpenApiCode>(result.Messages);

        IEnumerable<IValidationMessage> Messages(OpenApiDiagnostic diagnostic) => diagnostic.Errors
            .Select(e => readerOptions.IgnoreErrors
                ? ValidationMessage.Warn(e.Message, e.Pointer)
                : ValidationMessage.Error(e.Message, e.Pointer))
            .Concat(diagnostic.Warnings
                .Where(e => e.Message != "Data and type mismatch found.")
                .Select(e => ValidationMessage.Warn(e.Message, e.Pointer)));
    }

    private sealed record ReadResult(OpenApiDocument Document, OpenApiDiagnostic Diagnostic);
}
