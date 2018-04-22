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
    }
}