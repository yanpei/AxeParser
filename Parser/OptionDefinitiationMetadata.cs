using System;

namespace Parser
{
    public interface IOptionDefinitionMetadata
    {
        string Description { get; }
        IOptionSymbolMetadata SymbolMetadata { get; }
    }

    class OptionDefinitiationMetadata : IOptionDefinitionMetadata
    {
        public string Description { get; }

        public IOptionSymbolMetadata SymbolMetadata { get; }

        internal OptionDefinitiationMetadata(string fullForm, char? abbrevationFormForm, string description)
        {
            ValidateFlagDefination(fullForm, abbrevationFormForm);

            SymbolMetadata = new OptionSymbolMetadata(fullForm, abbrevationFormForm);
            Description = description ?? string.Empty;
        }

        void ValidateFlagDefination(string fullForm, char? abbrevationForm)
        {
            if (fullForm == null && abbrevationForm == null)
            {
                throw new ArgumentException("should define full name or abbrevation");
            }
            if (fullForm != null && ValidateFullForm(fullForm))
            {
                throw new ArgumentException("invalid full name");
            }
            if (abbrevationForm != null && ValidateAbbrevationForm(abbrevationForm))
            {
                throw new ArgumentException("invalid abbrevation");
            }
        }

        public bool ValidateAbbrevationForm(char? abbrevationForm)
        {
            return !FlagRegex.AbbrevationFormRegex.IsMatch(abbrevationForm.ToString());
        }

        public bool ValidateFullForm(string fullForm)
        {
            return !FlagRegex.FullFormRegex.IsMatch(fullForm);
        }

        internal OptionDefinitiationMetadata GetFlag(string arg)
        {
            if (arg.StartsWith("--"))
            {
                var option = arg.Substring(2, arg.Length - 2);
                if (string.Equals(SymbolMetadata.FullForm, option, StringComparison.OrdinalIgnoreCase))
                {
                    return this;
                }
            }
            else if (arg.StartsWith("-"))
            {
                var option = arg.Substring(1, arg.Length - 1);

                if (string.Equals(SymbolMetadata.Abbreviation.ToString(), option, StringComparison.OrdinalIgnoreCase))
                {
                    return this;
                }
            }

            return null;

        }
    }
}