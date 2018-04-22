using System.Collections.Generic;

namespace Parser
{
    public interface ICommandDefinitionMetadata
    {
        string Symbol { get; }
        IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata();
    }
}