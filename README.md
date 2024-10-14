# Qowaiv Code Generation
A library that helps generating code.

# Open API
Generates models defined in [Open API schema's](https://swagger.io/specification/).
More info can be found [here](src/Qowaiv.CodeGeneration/OpenAPi/README.md).

# Single Value Objects
This project helps generating code for [Qowaiv Single Value Objects](https://github.com/Qowaiv/Qowaiv).


## C# writer settings
By specifying the C# writer settings, the output of the generated code can
be tweaked:

### GlobalUsings
Types written who's namespace occurs in the `GlobalUsings` is written without
it's (full) namespace declaration.

### NewLine
The new line character(s) to use. The default is `"\r\n"`.

### Indentation
The indentation to use. The default is four spaces `"    "`.

### Use required modifier
By setting `UseRequiredModifier = true`, the [`required` modifier](https://learn.microsoft.com/dotnet/csharp/language-reference/keywords/required)
is generated for required properties. Note that this language feature is only
available since C# 11.

### Encoding
The encoding to use. The default is [UTF-8 without BOM](https://en.wikipedia.org/wiki/UTF-8).
