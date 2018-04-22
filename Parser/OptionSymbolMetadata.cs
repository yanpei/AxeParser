namespace Parser
{
    internal interface IOptionSymbolMetadata
    {
        string FullForm { get; }
        char? AbbrevationForm { get; }
    }

    class OptionSymbolMetadata : IOptionSymbolMetadata
    {
        public OptionSymbolMetadata(string fullForm, char? abbrevationForm)
        {
            FullForm = fullForm;
            AbbrevationForm = abbrevationForm;
        }

        public string FullForm { get; }
        public char? AbbrevationForm { get; }
    }
}