using System;
using System.Collections.Generic;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// <example>ArgsParsingResult</example>
    /// </summary>
    public class ArgsParsingResult
    {
        /// <value>Bool. Indicate parsing reslut success or not.</value>
        public bool IsSuccess { get; set; }
        public ICommandDefinitionMetadata Command { get; internal set; }
        internal HashSet<OptionDefinitiationMetadata> FlagOptions { get; set; } = new HashSet<OptionDefinitiationMetadata>();
        /// <value>Error</value>
        public ParsingError Error { get; set; }

        /// <returns>True when flag was unsed when parsing</returns>
        /// <returns>Flase when flag was not unsed when parsing</returns>
        /// <param name="flag"></param>
        public bool GetFlagValue(string flag)
        {
            if (!IsSuccess)
            {
                throw new InvalidOperationException("only can get flag value when parse succeed");
            }
            if (string.IsNullOrEmpty(flag))
            {
                throw new ArgumentNullException(nameof(flag));
            }
            if (flag.StartsWith("--"))
            {
                var option = flag.Substring(2, flag.Length - 2);
                if (!FlagRegex.FullFormRegex.IsMatch(option))
                {
                    throw new ArgumentException("flag is invalid");
                }
                return FlagOptions.Any(f => string.Equals(f.SymbolMetadata.FullForm, option, StringComparison.OrdinalIgnoreCase));
            }
            if (flag.StartsWith("-"))
            {
                var option = flag.Substring(1, flag.Length - 1);
                if (!FlagRegex.AbbrevationFormRegex.IsMatch(option))
                {
                    throw new ArgumentException("flag is invalid");
                }
                return FlagOptions.Any(f => string.Equals(f.SymbolMetadata.Abbreviation.ToString(), option, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }
    }
}