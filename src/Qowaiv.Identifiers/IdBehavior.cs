namespace Qowaiv.Identifiers;

public interface IdBehavior<TId> where TId : IEquatable<TId>
{
    TId Next();
}

