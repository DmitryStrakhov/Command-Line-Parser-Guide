using System;
using System.Text;
using CommandLine;
using FluentAssertions;
using NUnit.Framework;

namespace CommandLineParserGuide;

[TestFixture]
public class Groups {
    [Test]
    public void Test() {
        string[] args = "--i1 1 --i4 4 --opt 3".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<Options>(args).WithParsed(opt => {
                traceBuilder.Append("WithParsed;");

                opt.Should().NotBeNull();
                opt.I1Option.Should().Be(1);
                opt.I2Option.Should().Be(0);
                opt.I3Option.Should().Be(0);
                opt.I4Option.Should().Be(4);
                opt.Opt.Should().Be(3);
            }
        );
        string trace = traceBuilder.ToString();
        trace.Should().Be("WithParsed;");
    }

    class Options {
        [Option(longName: "i1", Group = "G1", HelpText = "I1 Option Help")]
        public int I1Option { get; set; }
        
        [Option(longName: "i2", Group = "G1", HelpText = "I2 Option Help")]
        public int I2Option { get; set; }
        
        [Option(longName: "i3", Group = "G2", HelpText = "I3 Option Help")]
        public int I3Option { get; set; }

        [Option(longName: "i4", Group = "G2", HelpText = "I4 Option Help")]
        public int I4Option { get; set; }

        [Option(longName: "opt", HelpText = "Opt Help")]
        public int Opt { get; set; }
    }
}
