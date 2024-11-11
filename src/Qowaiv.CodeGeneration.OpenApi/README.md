# Qowaiv Code Generation Open API
Generates models defined in [Open API schema's](https://swagger.io/specification/).

## Open API type resolver settings
Settings to specify how to resolve types.

## Customization
By creation an override to `OpenApiTypeResolver` the generation can be
customized.

### Define a custom base class
It can be useful to override the base class that should will be set for a
resolved class/record:

``` C#
protected override Type? ResolveCustomization(ResolveOpenApiSchema schema) => schema switch
{
    _ when schema.Matches("ClassToMatch") => ResolveObject(schema).WithBase(typeof(Base), schema),
    _ => null,
};
```

An other way to achieve this:

``` C#
partial record ClassToMatch : Base { }

````

By using `ResolveCustomization` however, properties already defined in the base
class are not longer rendered, where in the second scenario, this is not the case.

By providing `null` as base, no base class will be added to the resolved type.
