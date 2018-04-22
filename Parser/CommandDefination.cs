using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class CommandDefination : ICommandDefinitionMetadata
    {
        public CommandDefination(string symbol) { Symbol = symbol; }
        public string Symbol { get; }
        internal HashSet<OptionDefinitiationMetadata> FlagOptions { get; set; } = new HashSet<OptionDefinitiationMetadata>();

        public IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata()
        {
            return FlagOptions.ToArray();
        }
    }
}