using System.Collections.Generic;

namespace Parser
{
    /// <summary>
    /// command informations
    /// </summary>
    public interface ICommandDefinitionMetadata
    {
        /// <summary>
        /// command symbol, for default command, will be null.
        /// </summary>
        string Symbol { get; }
        /// <summary>
        /// command description, if not defined, will be empty string
        /// </summary>
        string Description { get; }
        /// <summary>
        /// return all registered options of command
        /// </summary>
        /// <returns></returns>
        IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata();
    }
}