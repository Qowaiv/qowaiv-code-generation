namespace Qowaiv.CodeGeneration.Syntax;

[Inheritable]
public class Record : Class
{
    public Record(TypeInfo info) : base(info) { }

    /// <inheritdoc />
    protected override string Keyword => "record";
}
