using System.Reflection;

namespace Qowaiv.CodeGeneration;

internal static class TypeInfoExtensions
{
    public static void AddAttributes(this TypeInfo info, IEnumerable<AttributeInfo> attributes)
    {
        var list = (List<AttributeInfo>)info.Attributes;
        list.AddRange(attributes);
    }

    public static void AddDerivedType(this TypeInfo info, Type type)
    {
        var list = (List<Type>)info.DerivedTypes;
        list.Add(type);
    }

    public static void AddProperties(this TypeInfo info, IEnumerable<PropertyInfo> properties)
    {
        var list = (List<PropertyInfo>)info.Properties;
        list.AddRange(properties);
    }
}
