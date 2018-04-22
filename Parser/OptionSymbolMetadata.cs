namespace Parser
{
    public interface IOptionSymbolMetadata
    {
        string FullForm { get; }
        char? Abbreviation { get; }
    }

    class OptionSymbolMetadata : IOptionSymbolMetadata
    {
        public OptionSymbolMetadata(string fullForm, char? abbreviation)
        {
            FullForm = fullForm;
            Abbreviation = abbreviation;
        }

        public string FullForm { get; }
        public char? Abbreviation { get; }
    }
}