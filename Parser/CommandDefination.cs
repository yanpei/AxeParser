using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    public class CommandDefination : ICommandDefinitionMetadata
    {
        public CommandDefination(string symbol, string description)
        {
            Symbol = symbol;
            Description = description;
        }

        public string Symbol { get; }
        public string Description { get;}
        internal HashSet<OptionDefinitiationMetadata> FlagOptions { get; set; } = new HashSet<OptionDefinitiationMetadata>();


        public IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata()
        {
            return FlagOptions.ToArray();
        }
    }
}