﻿namespace Qowaiv.OpenApi.Collection;

public delegate string EnumValueNaming(string name);
public delegate string PropertyNaming(string name, Type model);

public static class NamingStrategy
{
    [Pure]
    public static string None(string name) => name;

    [Pure]
    public static string PascalCase(string name)
       => (char.ToUpperInvariant(name[0]) + name[1..]);

    [Pure]
    public static string PascalCase(string name, Type model) 
        => PascalCase(name).Enclosing(model);

    [Pure]
    public static string CamelCase(string name, Type model) 
        => (char.ToLowerInvariant(name[0]) + name[1..]).Enclosing(model);

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
