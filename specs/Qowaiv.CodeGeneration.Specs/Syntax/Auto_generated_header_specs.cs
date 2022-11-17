﻿using Qowaiv.CodeGeneration.Syntax;

namespace Syntax.Auto_generated_header_specs;

public class Content
{
    [Test]
    public void with_auto_generated_tag()
        => new AutoGeneratedHeader().Should().HaveContent(
           @"// ------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
// ------------------------------------------------------------------------------

");
}
