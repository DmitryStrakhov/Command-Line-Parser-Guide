using System.Text;
using CommandLine;
using FluentAssertions;
using NUnit.Framework;

namespace CommandLineParserGuide;

[TestFixture]
public class OptionsAndValuesTests {
    [Test]
    public void Test() {
        string[] args = "Val1 Val2 --int 3 -b -e Opt1 Opt2".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<Options>(args).WithParsed(opt => {
                traceBuilder.Append("WithParsed;");

                opt.Should().NotBeNull();
                opt.IntegerOption.Should().Be(3);
                opt.BooleanOption.Should().Be(true);
                opt.EnumerableOption.Should().Equal("Opt1", "Opt2");
                opt.Value1.Should().Be("Val1");
                opt.Value2.Should().Be("Val2");
            }
        );
        string trace = traceBuilder.ToString();
        trace.Should().Be("WithParsed;");
    }

    class Options {
        [Option(shortName: 'i', longName: "int", HelpText = "Integer Option Help")]
        public int IntegerOption { get; set; }

        [Option(shortName: 'b', longName: "bool", HelpText = "Boolean Option Help")]
        public bool BooleanOption { get; set; }

        [Option(shortName: 'e', longName: "enumerable", Min = 1, Max = 3, HelpText = "Enumeration Option Help")]
        public IEnumerable<string>? EnumerableOption { get; set; }

        [Value(0, HelpText = "Value1 Help")]
        public string? Value1 { get; set; }

        [Value(1, HelpText = "Value2 Help")]
        public string? Value2 { get; set; }
    }
}
