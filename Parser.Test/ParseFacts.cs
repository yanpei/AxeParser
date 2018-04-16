using System;
using Xunit;

namespace Parser.Test
{
    public class ParseFacts
    {
        [Fact]
        void should_parse_flag_successfully_by_full_name()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag1", 'f', "This is a flag").EndCommand().Build();
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
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("fLag", 'f', "This is a flag").EndCommand().Build();
            ArgsParsingResult result = parser.Parse(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue("--fLag"));
            Assert.True(result.GetFlagValue("-f"));
        }

        [Fact]
        void should_parse_flag_successfully_by_abbrevation_when_only_define_abbrevation()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption(null, 'f', "This is a flag").EndCommand().Build();
            ArgsParsingResult result = parser.Parse(new[] { "-f" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.True(result.GetFlagValue("-f"));
            Assert.False(result.GetFlagValue("--flag"));
        }

        [Fact]
        void should_parse_flag_successfully_by_full_name_when_only_define_full_name()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("_Fla26--182-g-", null, "This is a flag").EndCommand().Build();
            ArgsParsingResult result = parser.Parse(new[] { "--_Fla26--182-g-" });

            Assert.True(result.IsSuccess);
            Assert.Null(result.Error);
            Assert.False(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("--_Fla26--182-g-"));
        }


        [Theory]
        [InlineData("--flag", "--flag", "--flag")]
        [InlineData("--flag", "-f", "-f")]
        [InlineData("--FlAg", "-F", "-F")]
        void should_can_not_parse_duplicated_flag_at_one_time(string arg1, string arg2, string duplicatedArg)
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
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
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] {arg1, arg2});
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal(errorTrigger, result.Error.Trigger);
        }

        [Fact]
        void should_can_not_parse_undefined_flags()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] {"-v", "--flag", "--continue"});
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-v", result.Error.Trigger);
        }

        [Fact]
        void should_parse_failed_when_parse_continous_abbrevation()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] { "-rf" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-rf", result.Error.Trigger);
        }


        [Fact]
        void should_throw_ArgumentNullException_when_parse_null_args()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();

            Assert.Throws<ArgumentNullException>(() => parser.Parse(null));
        }

        [Fact]
        void should_throw_ArgumentException_when_parse_args_with_null()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();

            Assert.Equal("cannot parse args with null", Assert.Throws<ArgumentException>(() => parser.Parse(new []{"-f", null })).Message);
        }


        [Fact]
        void should_can_parse_multiple_flags()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').AddFlagOption("version", 'v').EndCommand().Build();
            var result = parser.Parse(new[] { "-f" , "-v"});
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-v"));
        }

        [Fact]
        void should_can_parse_combined_flags()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').AddFlagOption("version", 'v').EndCommand().Build();
            var result = parser.Parse(new[] { "-fv" });
            Assert.True(result.IsSuccess);
            Assert.True(result.GetFlagValue("-f"));
            Assert.True(result.GetFlagValue("-v"));
        }

        [Fact]
        void should_parse_failed_and_return_FreeValueNotSupported_error_when_given_combined_flags_contains_not_defined_flag()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] { "-fv" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.FreeValueNotSupported, result.Error.Code);
            Assert.Equal("-fv", result.Error.Trigger);
        }

        [Fact]
        void should_parse_failed_and_return_DuplicateFlagsInArgs_error_when_given_duplicate_flags()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').AddFlagOption(null,'v').EndCommand().Build();
            var result = parser.Parse(new[] { "-fv", "--flag" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.DuplicateFlagsInArgs, result.Error.Code);
            Assert.Equal("--flag", result.Error.Trigger);
        }

        [Fact]
        void should_parse_failed_and_return_DuplicateFlagsInArgs_error_when_given_combined_flags_contains_duplicated_flag()
        {
            var parser = new ArgsParserBuilder().BeginDefaultCommand().AddFlagOption("flag", 'f').EndCommand().Build();
            var result = parser.Parse(new[] { "-ff" });
            Assert.False(result.IsSuccess);
            Assert.Equal(ParsingErrorCode.DuplicateFlagsInArgs, result.Error.Code);
            Assert.Equal("-ff", result.Error.Trigger);
        }
    }
}
