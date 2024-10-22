namespace Qowaiv.CodeGeneration.Syntax;

/// <summary>Represents a record.</summary>
[Inheritable]
public class Record : Class
{
    /// <summary>Initializes a new instance of the <see cref="Record"/> class.</summary>
    public Record(TypeInfo info) : base(info) { }

    /// <summary>The record keyword.</summary>
    protected override string Keyword => "record";
}
