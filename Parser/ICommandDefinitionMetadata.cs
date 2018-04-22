using System.Collections.Generic;

namespace Parser
{
    public interface ICommandDefinitionMetadata
    {
        string Symbol { get; }
        string Description { get; }
        IEnumerable<IOptionDefinitionMetadata> GetRegisteredOptionsMetadata();
    }
}