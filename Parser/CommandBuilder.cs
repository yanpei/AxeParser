using System;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// build new command
    /// </summary>
    public class CommandBuilder
    {
        internal readonly CommandDefination commandDefination;
        readonly ArgsParserBuilder argsParserBuilder;

        public CommandBuilder(ArgsParserBuilder argsParserBuilder, string symbol, string description)
        {
            commandDefination = new CommandDefination(symbol, description);
            this.argsParserBuilder = argsParserBuilder;
        }

        /// <summary>
        /// Add a flag
        /// </summary>
        /// <param name="fullForm"></param>
        /// <param name="abbrevationForm"></param>
        /// <param name="description">Flag deacription.</param>
        /// <exception cref="ArgumentException">When add confict fullForm or abbrevationForm</exception>
        public CommandBuilder AddFlagOption(string fullForm, char? abbrevationForm, string description = null)
        {
            ValidateFullForm(fullForm);
            ValidateAbbrevationForm(abbrevationForm);

            commandDefination.FlagOptions.Add(new OptionDefinitiationMetadata(fullForm, abbrevationForm, description));
            return this;
        }

        void ValidateAbbrevationForm(char? abbrevationForm)
        {
            if (abbrevationForm != null &&
                commandDefination.FlagOptions.Any(
                    f => string.Equals(
                        f.SymbolMetadata.Abbreviation.ToString(),
                        abbrevationForm.ToString(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("conflict abbrevation form");
            }
        }

        void ValidateFullForm(string fullForm)
        {
            if (!string.IsNullOrEmpty(fullForm) &&
                commandDefination.FlagOptions.Any(f => string.Equals(f.SymbolMetadata.FullForm, fullForm, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("conflict full form");
            }
        }

        /// <summary>
        /// end the command defination, and put the command into the parser.
        /// </summary>
        /// <returns></returns>
        public ArgsParserBuilder EndCommand()
        {
            argsParserBuilder.SetDefaultCommand(this);
            return argsParserBuilder;
        }
    }
}