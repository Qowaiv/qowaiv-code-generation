using FluentAssertions;
using NUnit.Framework;
using Qowaiv.CodeGeneration;
using System;
using System.Reflection;

namespace CodeGeneration.Property_specs;

internal class Property_specs
{
    [Test]
    public void X()
    {
        var reference = typeof(Model).GetProperty(nameof(Model.Child));
        var property = new Property("Child", typeof(Model), typeof(Model), PropertyAccess.GetOnly);
        ((object)property).Should().BeEquivalentTo(reference);
    }

    [Test]
    public void enum_()
    {
        var type = typeof(System.StringComparison);
        var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

        var x = Enum.GetValues(type);

    }
}


internal class Model
{
    public Model? Child { get; }
}
