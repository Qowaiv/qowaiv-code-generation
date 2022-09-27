﻿using Qowaiv.OpenApi.IO;

namespace Qowaiv.OpenApi.Decoration;

public class SystemTextJsonDecorator : CodeDecorator
{
    public static readonly CodeType JsonConstructor = new("JsonConstructorAttribute", "System.Text.Json.Serialization");
    public static readonly CodeType JsonPropertyName = new("JsonPropertyNameAttribute", "System.Text.Json.Serialization");

    public override void Constructor(CSharpWriter writer, CodeModel model)
        => writer.Indent()
        .Write('[').Write(JsonConstructor, attribute: true).Line(']');

    public override void Property(CSharpWriter writer, CodeProperty property)
        => writer.Indent()
        .Write('[').Write(JsonPropertyName, attribute: true).Write('(').Literal(property.Schema.Name).Line(")]");
}