namespace Qowaiv.CodeGeneration;

public static class NamingStrategy
{
    [Pure]
    public static string None(string name) => name;

    [Pure]
    public static string PascalCase(string name)
       => (char.ToUpperInvariant(name[0]) + name[1..]);

    [Pure]
    public static string PascalCase(string name, Type type)
        => PascalCase(name).Enclosing(type);

    [Pure]
    public static string CamelCase(string name, Type type)
        => (char.ToLowerInvariant(name[0]) + name[1..]).Enclosing(type);

    [Pure]
    public static string Enum(string name)
    {
        if (name[0] >= '0' && name[0] <= '9')
        {
            name = 'N' + name;
        }
        name = name.Replace("+", "_pls");
        return name;
    }

    [Pure]
    private static string Enclosing(this string name, Type type)
        => type.Name == name
        ? '_' + name
        : name;

    [Pure]
    public static string EscapeKeywords(this string name)
        => keywords.Contains(name)
        ? "@" + name
        : name;

    private static readonly string[] keywords = new[] { "default" };
}
