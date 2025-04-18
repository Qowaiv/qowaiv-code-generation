namespace Qowaiv.CodeGeneration.SingleValueObjects;

public sealed class SvoParameters
{
    public required string Svo { get; init; }

    public required Namespace Namespace { get; init; }
    
    public required string Behavior { get; init; }
}
