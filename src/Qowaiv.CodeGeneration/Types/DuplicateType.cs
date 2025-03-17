namespace Qowaiv.CodeGeneration;

/// <summary>An exception indicating that the same type has been generated multiple times.</summary>
public class DuplicateType : Exception
{
    /// <summary>Initializes a new instance of the <see cref="DuplicateType"/> class.</summary>
    public DuplicateType() { }

    /// <summary>Initializes a new instance of the <see cref="DuplicateType"/> class.</summary>
    public DuplicateType(string? message) : base(message) { }

    /// <summary>Initializes a new instance of the <see cref="DuplicateType"/> class.</summary>
    public DuplicateType(string? message, Exception? innerException) : base(message, innerException) { }
}
