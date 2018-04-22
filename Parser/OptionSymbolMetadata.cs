namespace Parser
{
    /// <summary>
    /// option symbold metadata, including full form and abbreviation of option
    /// </summary>
    public interface IOptionSymbolMetadata
    {
        /// <summary>
        /// full form of option, if not define full form, will be null.
        /// </summary>
        string FullForm { get; }
        /// <summary>
        /// abbreviation of option, if not defnie abbreviaton, will be null.
        /// </summary>
        char? Abbreviation { get; }
    }

    internal class OptionSymbolMetadata : IOptionSymbolMetadata
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