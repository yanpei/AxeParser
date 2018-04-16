using System;
using System.Linq;

namespace Parser
{
    public class CommandBuilder
    {
        internal readonly ArgsParser parser;
        readonly ArgsParserBuilder argsParserBuilder;

        public CommandBuilder(ArgsParserBuilder argsParserBuilder, string symbol)
        {
            parser = new ArgsParser(new CommandDefination(symbol));
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

            parser.FlagOptions.Add(new FlagOption(fullForm, abbrevationForm, description));
            return this;
        }

        void ValidateAbbrevationForm(char? abbrevationForm)
        {
            if (abbrevationForm != null &&
                parser.FlagOptions.Any(
                    f => string.Equals(
                        f.AbbrevationForm.ToString(),
                        abbrevationForm.ToString(),
                        StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("conflict abbrevation form");
            }
        }

        void ValidateFullForm(string fullForm)
        {
            if (!string.IsNullOrEmpty(fullForm) &&
                parser.FlagOptions.Any(f => string.Equals(f.FullForm, fullForm, StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException("conflict full form");
            }
        }

        public ArgsParserBuilder EndCommand()
        {
            if (argsParserBuilder.DefaultCommandBuilder != null)
            {
                throw new InvalidOperationException("Default Command is arleady defined");
            }

            argsParserBuilder.DefaultCommandBuilder = this;
            return argsParserBuilder;
        }
    }
}