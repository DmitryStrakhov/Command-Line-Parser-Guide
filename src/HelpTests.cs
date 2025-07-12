using System.Text;
using CommandLine;
using CommandLine.Text;
using NUnit.Framework;

namespace CommandLineParserGuide;

[TestFixture]
public class HelpTests {
    [Test, Explicit]
    public void Test() {
        string[] args = "--invalid-command".Split();
        StringBuilder traceBuilder = new();

        Parser parser = new(settings => {
            settings.HelpWriter = null;
        });
        
        var parserResult = parser.ParseArguments<Options>(args);
        parserResult
            .WithParsed(options => {
                traceBuilder.Append("WithParsed;");
            })
            .WithNotParsed(errors => {
                traceBuilder.Append("WithNotParsed;");
                
                HelpText helpText = HelpText.AutoBuild(parserResult, help => {
                    // customizations here
                    help.AdditionalNewLineAfterOption = false;
                    help.Heading = "MyApp";
                    help.Copyright = "Copyright";
                    return help;
                });
                TestContext.WriteLine(helpText);
            });

        string trace = traceBuilder.ToString();
        Assert.That(trace, Is.EqualTo("WithNotParsed;"));
    }

    class Options {
        [Option(shortName: 'i', longName: "int", HelpText = "Integer Option Help")]
        public int IntegerOption { get; set; }

        [Option(shortName: 'b', longName: "bool", HelpText = "Boolean Option Help")]
        public bool BooleanOption { get; set; }

        [Option(shortName: 'e', longName: "enumerable", Min = 1, Max = 3, HelpText = "Enumeration Option Help")]
        public IEnumerable<string>? EnumerableOption { get; set; }

        [Value(0, HelpText = "Value1 Help")] public string? Value1 { get; set; }

        [Value(1, HelpText = "Value2 Help")] public string? Value2 { get; set; }

        [Usage(ApplicationAlias = "myapp")]
        public static IEnumerable<Example> Examples {
            get {
                yield return new("Usage Text",
                    new Options {
                        BooleanOption = true,
                        IntegerOption = 1,
                        EnumerableOption = ["opt1", "opt2"],
                        Value1 = "Val1",
                        Value2 = "Val2"
                    }
                );
            }
        }
    }
}
