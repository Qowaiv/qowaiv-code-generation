namespace Qowaiv.CodeGeneration;

/// <summary>Convention on how to write a name describing code.</summary>
public abstract class CodeNameConvention
{
    /// <summary>No naming convention.</summary>
    public static readonly CodeNameConvention None = new NoCovention();

    /// <summary>Parts are written without spaces with capitalized words.</summary>
    public static readonly CodeNameConvention PascalCase = new PascalCasing();

    /// <summary>Parts are written without spaces with capitalized words, starting lowercase.</summary>
    public static readonly CodeNameConvention CamelCase = new CamelCasing();

    /// <summary>Parts are written lowercase and connected using hyphens.</summary>
    public static readonly CodeNameConvention KebabCase = new KebabCasing();

    /// <summary>Parts are written lowercase and connected using underscores.</summary>
    public static readonly CodeNameConvention SnakeCase = new SnakeCasing();

    /// <summary>Parts are written uppercase and connected using underscores.</summary>
    public static readonly CodeNameConvention ScreamingSnakeCase = new ScreamingSnakeCasing();

    /// <summary>Parts are written as is connected with spaces.</summary>
    public static readonly CodeNameConvention SentenceCase = new SentenceCasing();

    /// <summary>The name of the convention.</summary>
    public abstract string Name { get; }

    /// <summary>Represents the parts as string according to the naming convention.</summary>
    [Pure]
    public abstract string ToString(IReadOnlyCollection<string> parts);

    /// <summary>Splits the string in parts according to the naming convention.</summary>
    [Pure]
    public IReadOnlyCollection<string> Split(string? part) => Split([part]);

    /// <summary>Splits the parts in parts according to the naming convention.</summary>
    [Pure]
    public abstract IReadOnlyCollection<string> Split(IEnumerable<string?> parts);

    /// <summary>Applies formatting to the parts.</summary>
    [Pure]
    private static IEnumerable<string> Trim(IEnumerable<string?> parts) => parts.OfType<string>().Where(p => p is { Length: > 0 });

    private class PascalCasing : CodeNameConvention
    {
        /// <inheritdoc />
        public override string Name => "Pascal case";

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts)
            => Trim(parts.SelectMany(SplitPart))
            .Select((p, i) => i == 0 ? Formatted(p, i) : p)
            .ToArray();

        [Pure]
        private IEnumerable<string> SplitPart(string part)
        {
            part ??= string.Empty;
            var i = 0;
            var upper = 0;

            while (++i < part.Length)
            {
                if (char.IsUpper(part[i]) && !char.IsUpper(part[i - 1]))
                {
                    yield return part[upper..i];
                    upper = i;
                }
                // multiple Uppercases
                else if (!char.IsUpper(part[i]) && char.IsUpper(part[i - 1]))
                {
                    yield return part[upper..(i - 1)];
                    upper = i - 1;
                }
            }
            yield return part[upper..];
        }

        /// <inheritdoc />
        [Pure]
        public sealed override string ToString(IReadOnlyCollection<string> parts) => string.Concat(Trim(parts).Select(Formatted));

        [Pure]
        protected virtual string Formatted(string part, int index) => char.ToUpperInvariant(part[0]) + part[1..];
    }

    private sealed class CamelCasing : PascalCasing
    {
        /// <inheritdoc />
        public override string Name => "Camel case";

        [Pure]
        protected override string Formatted(string part, int index)
        {
            if (index == 0)
            {
                var start = new string(part.TakeWhile(char.IsUpper).ToArray());
                return start.ToLowerInvariant() + part[start.Length..];
            }
            else return base.Formatted(part, index);
        }
    }

    private sealed class KebabCasing : CodeNameConvention
    {
        /// <inheritdoc />
        public override string Name => "Kebab case";

        /// <inheritdoc />
        [Pure]
        public override string ToString(IReadOnlyCollection<string> parts) => string.Join('-', Trim(parts).Select(p => p.ToLowerInvariant()));

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts) 
            => Trim(parts.SelectMany(p => (p ?? string.Empty).Split('-')))
                .Select(p => p.ToLowerInvariant())
                .ToArray();
    }

    [Inheritable]
    private class SnakeCasing : CodeNameConvention
    {
        /// <inheritdoc />
        public override string Name => "Snake case";

        /// <inheritdoc />
        [Pure]
        public override string ToString(IReadOnlyCollection<string> parts) => string.Join('_', Trim(parts).Select(p => p.ToLowerInvariant()));

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts)
            => Trim(parts.SelectMany(p => (p ?? string.Empty).Split('_')))
                .Select(p => p.ToLowerInvariant())
                .ToArray();
    }

    private sealed class ScreamingSnakeCasing : SnakeCasing
    {
        /// <inheritdoc />
        public override string Name => "Screaming snake case";

        /// <inheritdoc />
        [Pure]
        public override string ToString(IReadOnlyCollection<string> parts) => base.ToString(parts).ToUpperInvariant();

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts)
            => base.Split(parts)
                .Select(p => p.ToUpperInvariant())
                .ToArray();
    }

    private sealed class SentenceCasing : CodeNameConvention
    {
        private readonly char[] Splitters = [' ', '_', '\t', '\r', '\n', (char)160];

        /// <inheritdoc />
        public override string Name => "Sentence case";

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts)
            => Trim(parts.SelectMany(p => (p ?? string.Empty).Split(Splitters)))
                .ToArray();

        /// <inheritdoc />
        [Pure]
        public override string ToString(IReadOnlyCollection<string> parts) => string.Join(' ', Trim(parts));
    }

    private sealed class NoCovention : CodeNameConvention
    {
        /// <inheritdoc />
        public override string Name => "None";

        /// <inheritdoc />
        [Pure]
        public override IReadOnlyCollection<string> Split(IEnumerable<string?> parts) => Trim(parts).ToArray();

        /// <inheritdoc />
        [Pure]
        public override string ToString(IReadOnlyCollection<string> parts) => string.Concat(Trim(parts));
    }
}
