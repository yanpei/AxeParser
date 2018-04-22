using System;
using Xunit;

namespace Parser.Test
{
    public class GetFlagValueFacts
    {
        [Theory]
        [InlineData("")]
        [InlineData(null)]
        void should_throw_ArgumentNullException_when_getValue(string flag)
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var argsParsingResult = parser.Parse(new[] {"--flag"});
            Assert.Throws<ArgumentNullException>(() => argsParsingResult.GetFlagValue(flag));
        }

        [Fact]
        void should_throw_InvalidOperationException_when_getValue_but_parse_failed()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var argsParsingResult = parser.Parse(new[] { "-v" });
            Assert.False(argsParsingResult.IsSuccess);
            Assert.Equal("only can get flag value when parse succeed", Assert.Throws<InvalidOperationException>(() => argsParsingResult.GetFlagValue("-v")).Message);
        }

        [Theory]
        [InlineData("-rf")]
        [InlineData("--")]
        [InlineData("-1")]
        [InlineData("- ")]
        [InlineData("-$")]
        [InlineData("---flag")]
        [InlineData("--fl$g")]
        void should_throw_ArgumentException_when_get_invalid_flag_value(string flag)
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] { "-f" });
            Assert.True(result.IsSuccess);
            Assert.Equal("flag is invalid", Assert.Throws<ArgumentException>(() => result.GetFlagValue(flag)).Message);
        }

        [Fact]
        void should_throw_ArgumentException_when_get_flag_value_of_combined_flags()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').AddFlagOption(null, 'v').EndCommand().Build();
            var result = parser.Parse(new[] { "-fv" });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-v"));
            Assert.Equal("flag is invalid", Assert.Throws<ArgumentException>(() => result.GetFlagValue("-fv")).Message);
        }

    }
}
