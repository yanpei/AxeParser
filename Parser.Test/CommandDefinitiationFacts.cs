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
    }
}