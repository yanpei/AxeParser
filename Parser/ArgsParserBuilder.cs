using System;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// To build a agrs builder
    /// </summary>
    public class ArgsParserBuilder
    {
        readonly ArgsParser parser;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArgsParserBuilder()
        {
            parser = new ArgsParser();
        }

        /// <summary>
        /// Add a flag
        /// </summary>
        /// <param name="fullForm"></param>
        /// <param name="abbrevationForm"></param>
        /// <param name="description">Flag deacription.</param>
        /// <exception cref="ArgumentException">When add confict fullForm or abbrevationForm</exception>
        public ArgsParserBuilder AddFlagOption(string fullForm, char? abbrevationForm, string description = null)
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

        /// <returns>Parser</returns>
        public ArgsParser Build()
        {
            return parser;
        }
    }

}