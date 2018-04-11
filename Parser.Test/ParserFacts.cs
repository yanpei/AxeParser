using System;
using Xunit;

namespace Parser.Test
{
    public class ParserFacts
    {
        [Fact]
        void should_parse_flag_successfully_by_full_name()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag1", 'f', "This is a flag").Build();
            ArgsParsingResult result = parser.Parse(new[] {"--flag1"});

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue("--flag1"));
            Assert.True(result.GetFlagValue("--FlAg1"));
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-F"));
        }

        [Fact]
        void should_parse_flag_successfully_by_abbrevation()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("fLag", 'f', "This is a flag").Build();
            ArgsParsingResult result = parser.Parse(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue("--fLag"));
            Assert.True(result.GetFlagValue("-f"));
        }

        [Fact]
        void should_parse_flag_successfully_by_abbrevation_when_only_define_abbrevation()
        {
            var parser = new ArgsParserBuilder().AddFlagOption(null, 'f', "This is a flag").Build();
            ArgsParsingResult result = parser.Parse(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue("-f"));
            Assert.False(result.GetFlagValue("--flag"));
        }

        [Fact]
        void should_parse_flag_successfully_by_full_name_when_only_define_full_name()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("_Fla26--182-g-", null, "This is a flag").Build();
            ArgsParsingResult result = parser.Parse(new[] { "--_Fla26--182-g-" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.False(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("--_Fla26--182-g-"));
        }

        [Fact]
        void should_contain_full_name_or_abbrevation_at_least()
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption(null, null, "This is a description."));
        }

        [Theory]
        [InlineData("-flag")]
        [InlineData("")]
        [InlineData("fl$g")]
        void should_match_full_name_define_rule(string fullName)
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption(fullName, 'f', "This is a description."));
        }

        [Theory]
        [InlineData('-')]
        [InlineData('1')]
        [InlineData(' ')]
        [InlineData('$')]
        void should_match_abbrevation_define_rule(char abbrevationForm)
        {
            Assert.Throws<ArgumentException>(() => new ArgsParserBuilder().AddFlagOption("flag", abbrevationForm, "This is a description."));
        }

        [Theory]
        [InlineData("--flag", "--flag", "--flag")]
        [InlineData("--flag", "-f", "-f")]
        [InlineData("--FlAg", "-F", "-F")]
        void should_can_not_parse_duplicated_flag_at_one_time(string arg1, string arg2, string duplicatedArg)
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] { arg1, arg2 });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.DuplicateFlagsInArgs, result.Error.Code);
            Assert.Equal(duplicatedArg, result.Error.Trigger);
        }

        [Theory]
        [InlineData("--flag", "-v", "-v")]
        [InlineData("-v", "--flag", "-v")]
        void should_can_not_parse_undefined_flag(string arg1, string arg2, string errorTrigger)
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] {arg1, arg2});
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal(errorTrigger, result.Error.Trigger);
        }

        [Fact]
        void should_can_not_parse_undefined_flags()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] {"-v", "--flag", "--continue"});
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-v", result.Error.Trigger);
        }

        [Fact]
        void should_parse_failed_when_parse_continous_abbrevation()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] { "-rf" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-rf", result.Error.Trigger);
        }

        [Theory]
        [InlineData("--flag")]
        [InlineData("-f")]
        [InlineData("--version")]
        [InlineData("-v")]
        void should_can_add_multiple_flags(string arg)
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag",  'f').AddFlagOption("version", 'v').Build();
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
            var builder = new ArgsParserBuilder().AddFlagOption(fullForm1, abbrevationForm1);

            Assert.Equal(errorMessage, Assert.Throws<ArgumentException>(() => builder.AddFlagOption(fullForm2, abbrevationForm2)).Message);
        }

        [Fact]
        void should_throw_ArgumentNullException_when_parse_null_args()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();

            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        void should_throw_ArgumentException_when_parse_args_with_null()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();

            Assert.Equal("cannot parse args with null", Assert.Throws<ArgumentException>(() => parser.Parse(new []{"-f", null })).Message);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        void should_throw_ArgumentNullException_when_getValue(string flag)
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var argsParsingResult = parser.Parse(new[] {"--flag"});
            Assert.Throws<ArgumentNullException>(() => argsParsingResult.GetFlagValue(flag));
        }

        [Fact]
        void should_throw_InvalidOperationException_when_getValue_but_parse_failed()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
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
        [InlineData("--")]
        [InlineData("--fl$g")]
        void should_throw_ArgumentException_when_get_invalid_flag_value(string flag)
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] { "-f" });
            Assert.True(result.IsSuccess);
            Assert.Equal("flag is invalid", Assert.Throws<ArgumentException>(() => result.GetFlagValue(flag)).Message);
        }

        [Fact]
        void should_can_parse_multiple_flags()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').AddFlagOption("version", 'v').Build();
            var result = parser.Parse(new[] { "-f" , "-v"});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-v"));
        }

        [Fact]
        void should_can_parse_combined_flags()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').AddFlagOption("version", 'v').Build();
            var result = parser.Parse(new[] { "-fv" });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-v"));
        }

        [Fact]
        void should_parse_failed_and_return_FreeValueNotSupported_error_when_given_combined_flags_contains_not_defined_flag()
        {
            var parser = new ArgsParserBuilder().AddFlagOption("flag", 'f').Build();
            var result = parser.Parse(new[] { "-fv" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-fv", result.Error.Trigger);
        }

    }
}
