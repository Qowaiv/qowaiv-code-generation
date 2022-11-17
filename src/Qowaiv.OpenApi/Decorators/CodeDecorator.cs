using Microsoft.OpenApi.Models;
using Qowaiv.CodeGeneration.Syntax;

namespace Qowaiv.OpenApi.Decorators;

public class CodeDecorator
{
    public virtual IEnumerable<Type> Interfaces => Array.Empty<Type>();

    [Pure]
    public virtual IEnumerable<AttributeInfo> Class(Class @class, OpenApiSchema schema) => Array.Empty<AttributeInfo>();

    [Pure]
    public virtual IEnumerable<AttributeInfo> Enum(Enumeration enumeration, OpenApiSchema schema) => Array.Empty<AttributeInfo>();

    [Pure]
    public virtual IEnumerable<AttributeInfo> Ctor(Class @class, OpenApiSchema schema) => Array.Empty<AttributeInfo>();

    [Pure]
    public virtual IEnumerable<AttributeInfo> Property(Property property, OpenApiProperty schema) => Array.Empty<AttributeInfo>();
}
