namespace Parser
{
    public class CommandDefination : ICommandDefinitionMetadata
    {
        public CommandDefination(string symbol) { Symbol = symbol; }
        public string Symbol { get; }
    }
}