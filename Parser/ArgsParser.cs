using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Parser
{
    /// <summary>
    /// Provide Parse method to parse args.
    /// </summary>
    public class ArgsParser
    {
        internal HashSet<FlagOption> FlagOptions { get; set; } = new HashSet<FlagOption>();
        Regex CombinedFlagsRegex = new Regex("^-[A-Za-z]*$");

        /// <summary>
        /// Validate args, will throw InvalidOperationException when parsing duplicated args.
        /// <returns>
        /// Parse args, will return ArgsParsingResult with error code and trigger reason when parsing undefined args.
        /// Return IsSuccess true when parsing successfully
        /// </returns>
        /// </summary>
        /// <param name="args">A string array which are need to be parsed</param>
        public ArgsParsingResult Parse(string[] args)
        {
            ValidateArgs(args);
            var duplicatedArgs = ValidateDuplicatedArgs(args);
            if (duplicatedArgs != null)
            {
                var result = new ArgsParsingResult();
                result.IsSuccess = false;
                result.Error = new ParsingError(ParsingErrorCode.DuplicateFlagsInArgs, duplicatedArgs);
                return result;
            }

            return ParseArgs(args);
        }

        ArgsParsingResult ParseArgs(string[] args)
        {
            var result = new ArgsParsingResult();
            foreach (var arg in args)
            {
                if (CombinedFlagsRegex.IsMatch(arg))
                {
                    var combinedArgs = arg.Substring(1, arg.Length - 1);
                    string[] splitedArgs = combinedArgs.Select(c => $"-{c}").ToArray();
                    foreach (var a in splitedArgs)
                    {
                        var flagOption = GetFlag(a);
                        if (flagOption == null)
                        {
                            result.IsSuccess = false;
                            result.Error = new ParsingError(ParsingErrorCode.FreeValueNotSupported, arg);
                            return result;
                        }
                        result.FlagOptions.Add(flagOption);
                    }

                    result.IsSuccess = true;
                }
                else
                {
                    var flagOption = GetFlag(arg);
                    if (flagOption == null)
                    {
                        result.IsSuccess = false;
                        result.Error = new ParsingError(ParsingErrorCode.FreeValueNotSupported, arg);
                        return result;
                    }
                    result.FlagOptions.Add(flagOption);
                    result.IsSuccess = true;
                }

            }

            return result;
        }

        FlagOption GetFlag(string arg)
        {
            return FlagOptions.FirstOrDefault(flag => flag.GetFlag(arg) != null);
        }

        void ValidateArgs(string[] args)
        {
            if (args == null)
            {
                throw new ArgumentNullException(nameof(args));
            }

            if (args.Any(arg => arg == null))
            {
                throw new ArgumentException("cannot parse args with null");
            }

        }

        string ValidateDuplicatedArgs(string[] args)
        {
            var parsedFlagOptions = new HashSet<FlagOption>();
            foreach (var arg in args)
            {
                var flagOption = GetFlag(arg);
                if (flagOption != null && !parsedFlagOptions.Add(flagOption))
                {
                    return arg;
                }
            }

            return null;
        }
    }

    /// <summary>
    /// <example>ArgsParsingResult</example>
    /// </summary>
    public class ArgsParsingResult
    {
        /// <value>Bool. Indicate parsing reslut success or not.</value>
        public bool IsSuccess { get; set; }
        internal HashSet<FlagOption> FlagOptions { get; set; } = new HashSet<FlagOption>();
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
                return FlagOptions.Any(f => string.Equals(f.FullForm, option, StringComparison.OrdinalIgnoreCase));
            }
            else if (flag.StartsWith("-"))
            {
                var option = flag.Substring(1, flag.Length - 1);
                if (!FlagRegex.AbbrevationFormRegex.IsMatch(option))
                {
                    throw new ArgumentException("flag is invalid");
                }
                return FlagOptions.Any(f => string.Equals(f.AbbrevationForm.ToString(), option, StringComparison.OrdinalIgnoreCase));
            }
            return false;
        }
    }
}