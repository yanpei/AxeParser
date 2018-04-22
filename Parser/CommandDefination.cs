using System.Collections.Generic;

namespace Parser
{
    public class CommandDefination : ICommandDefinitionMetadata
    {
        public CommandDefination(string symbol) { Symbol = symbol; }
        public string Symbol { get; }
        internal HashSet<OptionDefinitiationMetadata> FlagOptions { get; set; } = new HashSet<OptionDefinitiationMetadata>();

    }
}