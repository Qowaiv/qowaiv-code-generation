using System.Linq;

namespace Qowaiv.OpenApi.Collection;

public delegate string EnumValueNaming(string name);
public delegate string PropertyNaming(string name, CodeType model);

public static class NamingStrategy
{
    public static string None(string name) => name;

    public static string PascalCase(string name)
       => (char.ToUpperInvariant(name[0]) + name[1..]);

    public static string PascalCase(string name, CodeType model) 
        => PascalCase(name).Enclosing(model);

    public static string CamelCase(string name, CodeType model) 
        => (char.ToLowerInvariant(name[0]) + name[1..]).Enclosing(model);

    public static string Enum(string name)
    {
        if (name[0] >= '0' && name[0] <= '9')
        {
            name = 'N' + name;
        }
        name = name.Replace("+", "_pls");
        return name;
    }

    private static string Enclosing(this string name, CodeType type)
        => type.Name == name
        ? '_' + name
        : name;

    public static string EscapeKeywords(this string name)
        => keywords.Contains(name)
        ? "@" + name
        : name;


    private static readonly string[] keywords = new[] { "default" };
}
