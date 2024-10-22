using System.Reflection;

namespace Qowaiv.CodeGeneration;

internal static class TypeInfoExtensions
{
    public static void AddAttributes(this TypeInfo info, IEnumerable<AttributeInfo> attributes)
    {
        var list = (List<AttributeInfo>)info.Attributes;
        list.AddRange(attributes);
    }

    [Diagnostics.Contracts.CollectionMutation]
    public static bool AddDerivedType(this TypeInfo info, Type type)
    {
        var list = (List<Type>)info.DerivedTypes;
        if (!list.Exists(t => t.FullName == type.FullName))
        {
            list.Add(type);
            return true;
        }
        else
        {
            return false;
        }
    }

    public static void AddProperties(this TypeInfo info, IEnumerable<PropertyInfo> properties)
    {
        var list = (List<PropertyInfo>)info.Properties;
        list.AddRange(properties.Where(prop => !list.Exists(p => p.Name == prop.Name)));
    }
}
