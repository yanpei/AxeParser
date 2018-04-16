using System;
using Xunit;

namespace Parser.Test
{
    public class AddFlagsFacts
    {
        [Fact]
        void should_contain_full_name_or_abbrevation_at_least()
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption(null, null, "This is a description."));
        }

        [Theory]
        [InlineData("-flag")]
        [InlineData("")]
        [InlineData("fl$g")]
        void should_match_full_name_define_rule(string fullName)
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption(fullName, 'f', "This is a description."));
        }

        [Theory]
        [InlineData('-')]
        [InlineData('1')]
        [InlineData(' ')]
        [InlineData('$')]
        void should_match_abbrevation_define_rule(char abbrevationForm)
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", abbrevationForm, "This is a description."));
        }

        [Theory]
        [InlineData("--flag")]
        [InlineData("-f")]
        [InlineData("--version")]
        [InlineData("-v")]
        void should_can_add_multiple_flags(string arg)
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag",  'f').AddFlagOption("version", 'v').EndCommand().Build();
            ArgsParsingResult result = parser.Parse(new[] { arg });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue(arg));
        }

        [Theory]
        [InlineData("flag",'f', "flag", 'v', "conflict full form")]
        [InlineData("flag",'f', "Flag", 'v', "conflict full form")]
        [InlineData("flag",'f', "version", 'F', "conflict abbrevation form")]
        [InlineData("flag",'f', "version", 'f', "conflict abbrevation form")]
        void should_throw_ArgumentException_when_add_conflict_flag(string fullForm1, char? abbrevationForm1, string fullForm2, char? abbrevationForm2, string errorMessage)
        {
            var builder = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption(fullForm1, abbrevationForm1);

            Assert.Equal(errorMessage, Assert.Throws<ArgumentException>(() => builder.AddFlagOption(fullForm2, abbrevationForm2)).Message);
        }
    }
}
