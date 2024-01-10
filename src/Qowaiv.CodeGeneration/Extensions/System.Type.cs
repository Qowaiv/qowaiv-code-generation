namespace System;

public static class QowaivCodeGenerationTypeExtensions
{
    /// <summary>Returns true if the type is nullable value type.</summary>
    [Pure]
    public static bool IsNullableValueType(this Type type)
        => type is { IsValueType: true, IsGenericType: true }
        && type.GetGenericTypeDefinition() == typeof(Nullable<>);
}
