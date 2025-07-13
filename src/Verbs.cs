using System;
using System.Text;
using CommandLine;
using FluentAssertions;
using NUnit.Framework;

namespace CommandLineParserGuide;

[TestFixture]
public class Verbs {
    [Test]
    public void OpenVerbTest() {
        string[] args = @"open c:\temp -f -b".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<OpenVerbOptions, CloseVerbOptions>(args).MapResult(
            (OpenVerbOptions openOpt) => {
                traceBuilder.Append("OpenVerb;");

                openOpt.Should().NotBeNull();
                openOpt.Path.Should().Be(@"c:\temp");
                openOpt.FOption.Should().BeTrue();
                openOpt.BOption.Should().BeTrue();
                return 0;
            },
            (CloseVerbOptions closeOpt) => {
                traceBuilder.Append("CloseVerb;");
                return 0;
            },
            errors => {
                traceBuilder.Append("Error;");
                return 1;
            });
        string trace = traceBuilder.ToString();
        trace.Should().Be("OpenVerb;");
    }
    [Test]
    public void CloseVerbTest() {
        string[] args = "close -c".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<OpenVerbOptions, CloseVerbOptions>(args).MapResult(
            (OpenVerbOptions openOpt) => {
                traceBuilder.Append("OpenVerb;");
                return 0;
            },
            (CloseVerbOptions closeOpt) => {
                traceBuilder.Append("CloseVerb;");

                closeOpt.COption.Should().BeTrue();
                return 0;
            },
            errors => {
                traceBuilder.Append("Error;");
                return 1;
            });
        string trace = traceBuilder.ToString();
        trace.Should().Be("CloseVerb;");
    }
    [Test]
    public void DefaultVerbTest() {
        string[] args = @"c:\temp -b".Split();
        StringBuilder traceBuilder = new();

        Parser.Default.ParseArguments<OpenVerbOptions, CloseVerbOptions>(args).MapResult(
            (OpenVerbOptions openOpt) => {
                traceBuilder.Append("OpenVerb;");

                openOpt.Should().NotBeNull();
                openOpt.Path.Should().Be(@"c:\temp");
                openOpt.FOption.Should().BeFalse();
                openOpt.BOption.Should().BeTrue();
                return 0;
            },
            (CloseVerbOptions closeOpt) => {
                traceBuilder.Append("CloseVerb;");
                return 0;
            },
            errors => {
                traceBuilder.Append("Error;");
                return 1;
            });
        string trace = traceBuilder.ToString();
        trace.Should().Be("OpenVerb;");
    }


    [Verb("open", isDefault:true, HelpText = "Open command")]
    class OpenVerbOptions {
        [Option(shortName: 'f', HelpText = "FOption Help")]
        public bool FOption { get; set; }

        [Option(shortName: 'b', HelpText = "BOption Help")]
        public bool BOption { get; set; }

        [Value(0, HelpText = "Path Help")]
        public string Path { get; set; }
    }

    [Verb("close", HelpText = "Close command")]
    class CloseVerbOptions {
        [Option(shortName: 'c', HelpText = "COption Help")]
        public bool COption { get; set; }
    }
}
