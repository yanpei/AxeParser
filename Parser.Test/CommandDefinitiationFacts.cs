using System;
using System.Linq;
using Xunit;

namespace Parser.Test
{
    public class CommandDefinitiationFacts
    {
        [Fact]
        void should_return_registed_flag_options_of_comand()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', "flag description")
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new[] {"--flag"});
            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata[] optionDefinitionMetadatas = result.Command.GetRegisteredOptionsMetadata().ToArray();
            IOptionDefinitionMetadata flagMetadata = optionDefinitionMetadatas.Single(d => d.SymbolMetadata.FullForm.Equals("flag", StringComparison.OrdinalIgnoreCase));
            Assert.Equal("flag", flagMetadata.SymbolMetadata.FullForm);
            Assert.Equal('f', flagMetadata.SymbolMetadata.Abbreviation);
            Assert.Equal("flag description", flagMetadata.Description);
        }


        [Fact]
        void should_return_empty_when_get_registed_flags_of_comand_without_flags()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(Array.Empty<string>());

            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Command);
            IOptionDefinitionMetadata[] optionDefinitionMetadatas = result.Command.GetRegisteredOptionsMetadata().ToArray();
            Assert.Empty(optionDefinitionMetadatas);
        }

        [Fact]
        void should_return_empty_description_for_flag_not_defined_description()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', null)
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new[] { "--flag" });
            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata[] optionDefinitionMetadatas = result.Command.GetRegisteredOptionsMetadata().ToArray();
            IOptionDefinitionMetadata flagMetadata = optionDefinitionMetadatas.Single(d => d.SymbolMetadata.FullForm.Equals("flag", StringComparison.OrdinalIgnoreCase));
            Assert.Equal(string.Empty, flagMetadata.Description);
        }

        [Fact]
        void should_return_empty_description_for_default_command()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", 'f', "flag description")
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new[] { "--flag" });
            Assert.True(result.IsSuccess);
            Assert.Equal(string.Empty, result.Command.Description);             
        }

        [Fact]
        void should_return_null_when_get_full_form_of_flag_without_full_form()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption(null, 'f', null)
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new[] {"-f"});

            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata flagMetadata =
                result.Command.GetRegisteredOptionsMetadata()
                    .Single(d => d.SymbolMetadata.Abbreviation.ToString().Equals("f", StringComparison.OrdinalIgnoreCase));
            Assert.Null(flagMetadata.SymbolMetadata.FullForm);
        }

        [Fact]
        void should_return_null_when_get_abbreviation_of_flag_without_abbreviation()
        {
            ArgsParser parser = new ArgsParserBuilder()
                .BeginDefaultCommand()
                .AddFlagOption("flag", null, null)
                .EndCommand()
                .Build();
            ArgsParsingResult result = parser.Parse(new[] { "--flag" });

            Assert.True(result.IsSuccess);
            IOptionDefinitionMetadata flagMetadata =
                result.Command.GetRegisteredOptionsMetadata()
                    .Single(d => d.SymbolMetadata.FullForm.ToString().Equals("flag", StringComparison.OrdinalIgnoreCase));
            Assert.Null(flagMetadata.SymbolMetadata.Abbreviation);
        }
    }
}