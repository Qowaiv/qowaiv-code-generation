using Qowaiv.CodeGeneration;

namespace System.Reflection;

public static class QowaivEventInfoExtensions
{
    [Pure]  
    public static BindingFlags Bindings(this EventInfo @event)
    {
        Guard.NotNull(@event, nameof(@event));
        BindingFlags bindings = BindingFlags.Public | BindingFlags.Static;
        return bindings;
    }
}
