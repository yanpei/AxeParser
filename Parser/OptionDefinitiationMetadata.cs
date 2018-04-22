using System;
using System.Text.RegularExpressions;

namespace Parser
{
    internal interface IOptionDefinitionMetadata
    {
        string Description { get; }
        IOptionSymbolMetadata OptionSymbolMetadata { get; }
    }

    class OptionDefinitiationMetadata : IOptionDefinitionMetadata
    {
        public string Description { get; }

        public IOptionSymbolMetadata OptionSymbolMetadata { get; }

        internal OptionDefinitiationMetadata(string fullForm, char? abbrevationFormForm, string description)
        {
            ValidateFlagDefination(fullForm, abbrevationFormForm);

            OptionSymbolMetadata = new OptionSymbolMetadata(fullForm, abbrevationFormForm);
            Description = description;
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
                if (string.Equals(OptionSymbolMetadata.FullForm, option, StringComparison.OrdinalIgnoreCase))
                {
                    return this;
                }
            }
            else if (arg.StartsWith("-"))
            {
                var option = arg.Substring(1, arg.Length - 1);

                if (string.Equals(OptionSymbolMetadata.AbbrevationForm.ToString(), option, StringComparison.OrdinalIgnoreCase))
                {
                    return this;
                }
            }

            return null;

        }
    }
}