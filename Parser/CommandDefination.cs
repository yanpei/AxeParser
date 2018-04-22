using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// command defination
    /// </summary>
    public class CommandDefination : ICommandDefinitionMetadata
    {
        /// <summary>
        /// init command symbol and description
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="description"></param>
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