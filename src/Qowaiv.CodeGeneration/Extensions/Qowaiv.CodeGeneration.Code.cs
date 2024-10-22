using System.IO;
using System.Reflection;

namespace Qowaiv.CodeGeneration;

public static class CodeExtensions
{
    [Pure]
    public static string Stringify(this Code code)
    {
        Guard.NotNull(code);
        using var stream = new MemoryStream();
        var writer = new CSharpWriter(new StreamWriter(stream));
        code.WriteTo(writer);
        writer.Flush();
        return Encoding.UTF8.GetString(stream.ToArray());
    }

    [Pure]
    public static IEnumerable<TypeBase> IncludeUsings(this IEnumerable<TypeBase>? codes)
    {
        var types = new HashSet<TypeBase>(new TypeNameEqualityComparer());

        foreach (var type in Guard.NotNull(codes).OfType<TypeBase>())
        {
            types.Add(type);
            Visit(type, types);
        }

        return types;

        static void Visit(TypeBase type, ISet<TypeBase> visited)
        {
            foreach (var prop in type
                .GetProperties()
                .Select(PropertyTypeBase)
                .OfType<TypeBase>()
                .Where(visited.Add))
            {
                Visit(prop, visited);
            }

            if (type.BaseType is TypeBase @base && visited.Add(@base))
            {
                Visit(@base, visited);
            }

            foreach (var derived in type.GetDerivedTypes().OfType<TypeBase>().Where(visited.Add))
            {
                Visit(derived, visited);
            }
        }
        static TypeBase? PropertyTypeBase(PropertyInfo property)
        {
            var type = property.PropertyType as TypeBase;
            return type switch
            {
                null => null,
                _ when type.IsArray => type.GetElementType() as TypeBase,
                _ when type.IsNullableValueType() => type.GenericTypeArguments[0] as TypeBase,
                _ => type,
            };
        }
    }
}
