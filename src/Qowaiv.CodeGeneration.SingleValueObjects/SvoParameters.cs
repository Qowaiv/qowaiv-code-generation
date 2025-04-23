namespace Qowaiv.CodeGeneration.SingleValueObjects;

/// <summary>The parameters for the SVO generator.</summary>
public sealed record SvoParameters
{
    /// <summary>The (type) name of the SVO.</summary>
    public required string Svo { get; init; }

    /// <summary>Namespace of the SVO.</summary>
    public required Namespace Namespace { get; init; }

    /// <summary>The (full name) of the SVO behavior.</summary>
    public required string Behavior { get; init; }
}
