using System.Text;
using CommandLine;
using FluentAssertions;
using NUnit.Framework;

namespace CommandLineParserGuide;

[TestFixture]
public class MutuallyExclusiveTests {
    [Test]
    public void Test() {
        string[] args = "--i3 3 --i4 4 --opt 1".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<Options>(args).WithParsed(opt => {
                traceBuilder.Append("WithParsed;");

                opt.Should().NotBeNull();
                opt.I1Option.Should().Be(0);
                opt.I2Option.Should().Be(0);
                opt.I3Option.Should().Be(3);
                opt.I4Option.Should().Be(4);
                opt.Opt.Should().Be(1);
            }
        );
        string trace = traceBuilder.ToString();
        trace.Should().Be("WithParsed;");
    }

    class Options {
        [Option(longName: "i1", SetName = "G1", HelpText = "I1 Option Help")]
        public int I1Option { get; set; }

        [Option(longName: "i2", SetName = "G1", HelpText = "I2 Option Help")]
        public int I2Option { get; set; }

        [Option(longName: "i3", SetName = "G2", HelpText = "I3 Option Help")]
        public int I3Option { get; set; }

        [Option(longName: "i4", SetName = "G2", HelpText = "I4 Option Help")]
        public int I4Option { get; set; }

        [Option(longName: "opt", HelpText = "Opt Help")]
        public int Opt { get; set; }
    }    
}
