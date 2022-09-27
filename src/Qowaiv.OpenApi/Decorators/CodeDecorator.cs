using Microsoft.OpenApi.Models;
using Qowaiv.CodeGeneration;
using Qowaiv.CodeGeneration.IO;

namespace Qowaiv.OpenApi.Decorators;

public class CodeDecorator
{
    public virtual IEnumerable<Type> Interfaces => Array.Empty<Type>();

    [Pure]
    public virtual IEnumerable<Code> Class(Class @class, OpenApiSchema schema) => Array.Empty<Code>();

    [Pure]
    public virtual IEnumerable<Code> Enum(Enumeration enumeration, OpenApiSchema schema) => Array.Empty<Code>();

    [Pure]
    public virtual IEnumerable<Code> Ctor(Class @class, OpenApiSchema schema) => Array.Empty<Code>();

    [Pure]
    public virtual IEnumerable<Code> Property(Property property, OpenApiProperty schema) => Array.Empty<Code>();
}
