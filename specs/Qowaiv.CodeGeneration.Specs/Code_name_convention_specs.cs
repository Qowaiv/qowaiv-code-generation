namespace Code_name_convention_specs;

public class Splits
{
    [TestCase("HelloWorld", "Hello", "World")]
    [TestCase("helloWorld", "Hello", "World")]
    [TestCase("iAmNuts", "I", "Am", "Nuts")]
    [TestCase("XMLIsOld", "XML", "Is", "Old")]
    [TestCase("XMLIsOLD", "XML", "Is", "OLD")]
    public void With_Pascal_case(string str, params string[] parts)
        => CodeNameConvention.PascalCase.Split(str).Should().BeEquivalentTo(parts);

    [TestCase("HelloWorld", "hello", "World")]
    [TestCase("helloWorld", "hello", "World")]
    [TestCase("iAmNuts", "i", "Am", "Nuts")]
    [TestCase("XMLIsOld", "xml", "Is", "Old")]
    [TestCase("XMLIsOLD", "xml", "Is", "OLD")]
    public void With_Camel_case(string str, params string[] parts)
        => CodeNameConvention.CamelCase.Split(str).Should().BeEquivalentTo(parts);

    [TestCase("Hello-World", "hello", "world")]
    [TestCase("hello--World", "hello", "world")]
    public void With_Kebab_case(string str, params string[] parts)
        => CodeNameConvention.KebabCase.Split(str).Should().BeEquivalentTo(parts);

    [TestCase("Hello_World", "hello", "world")]
    [TestCase("hello__World", "hello", "world")]
    public void With_Snake_case(string str, params string[] parts)
        => CodeNameConvention.SnakeCase.Split(str).Should().BeEquivalentTo(parts);

    [TestCase("Hello_World", "HELLO", "WORLD")]
    [TestCase("hello__World", "HELLO", "WORLD")]
    public void With_Screaming_Snake_case(string str, params string[] parts)
        => CodeNameConvention.ScreamingSnakeCase.Split(str).Should().BeEquivalentTo(parts);

    [TestCase("Hello World", "Hello", "World")]
    [TestCase("Hello\tWorld", "Hello", "World")]
    [TestCase("Hello \tWorld", "Hello", "World")]
    [TestCase("Hello_World", "Hello", "World")]
    [TestCase("Hello World non-breaking", "Hello", "World", "non-breaking")]
    public void With_Sentence_case(string str, params string[] parts)
       => CodeNameConvention.SentenceCase.Split(str).Should().BeEquivalentTo(parts);
}

public class Formats
{
    [TestCase("HelloWorld", "hello", "World")]
    [TestCase("HelloWorld", "Hello", "World")]
    public void With_Pacal_case(string str, params string[] parts)
        => CodeNameConvention.PascalCase.ToString(parts).Should().Be(str);

    [TestCase("helloWorld", "hello", "World")]
    [TestCase("helloWorld", "Hello", "World")]
    public void With_Dromedary_case(string str, params string[] parts)
        => CodeNameConvention.CamelCase.ToString(parts).Should().Be(str);

    [TestCase("hello-world", "hello", "world")]
    [TestCase("hello-world", "Hello", "World")]
    public void With_Kebab_case(string str, params string[] parts)
        => CodeNameConvention.KebabCase.ToString(parts).Should().Be(str);

    [TestCase("hello_world", "hello", "world")]
    [TestCase("hello_world", "Hello", "World")]
    public void With_Snake_case(string str, params string[] parts)
        => CodeNameConvention.SnakeCase.ToString(parts).Should().Be(str);

    [TestCase("HELLO_WORLD", "hello", "world")]
    [TestCase("HELLO_WORLD", "Hello", "World")]
    public void With_Screaming_Snake_case(string str, params string[] parts)
        => CodeNameConvention.ScreamingSnakeCase.ToString(parts).Should().Be(str);

    [TestCase("Hello World", "Hello", "World")]
    public void With_Sentence_case(string str, params string[] parts)
       => CodeNameConvention.SentenceCase.ToString(parts).Should().Be(str);
}
