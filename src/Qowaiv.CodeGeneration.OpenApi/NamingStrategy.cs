namespace Qowaiv.OpenApi;

public delegate string PropertyNamingStrategy(string name, DotNetType type);

public static class NamingStrategy
{
    public static string None(string name) => name;
    public static string PascalCase(string name, DotNetType type) 
        => (char.ToUpperInvariant(name[0]) + name[1..]).Enclosing(type);
    public static string CamelCase(string name, DotNetType type) 
        => (char.ToLowerInvariant(name[0]) + name[1..]).Enclosing(type);

    public static string Enum(string name)
    {
        if (name[0] >= '0' && name[0] <= '9')
        {
            name = 'N' + name;
        }
        name = name.Replace("+", "_pls");
        return name;
    }

    private static string Enclosing(this string name, DotNetType type)
        => type.Name == name
        ? '_' + name
        : name;
}
