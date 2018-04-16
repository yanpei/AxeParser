using System;
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
        public ArgsParser(CommandDefination commandDefination)
        {
            if (commandDefination == null)
            {
                throw new ArgumentNullException(nameof(commandDefination));
            }
            command = commandDefination;
        }

        readonly CommandDefination command;

        internal HashSet<FlagOption> FlagOptions { get; set; } = new HashSet<FlagOption>();

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
            var duplicatedArgs = GetDuplicatedArgs(args);
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
            var needParseArgs = GetNeedParseArgs(args);

            foreach (var arg in needParseArgs)
            {
                var flagOption = GetFlag(arg.Arg);
                if (flagOption == null)
                {
                    result.IsSuccess = false;
                    result.Error = new ParsingError(ParsingErrorCode.FreeValueNotSupported, arg.Trigger);
                    return result;
                }

                result.FlagOptions.Add(flagOption);
            }

            result.IsSuccess = true;
            result.Command = command;
            return result;
        }

        List<ArgTrigger> GetNeedParseArgs(string[] args)
        {
            List<ArgTrigger> needParseArgs = new List<ArgTrigger>();
            foreach (var arg in args)
            {
                if (IsCombinedArgs(arg))
                {
                    var combinedArgs = arg.Substring(1, arg.Length - 1);
                    needParseArgs.AddRange(combinedArgs.Select(c => new ArgTrigger($"-{c}", arg)).ToArray());
                }
                else
                {
                    needParseArgs.Add(new ArgTrigger(arg, arg));
                }
            }
            return needParseArgs;
        }

        bool IsCombinedArgs(string arg)
        {
            return FlagRegex.CombinedFlagsRegex.IsMatch(arg);
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

        string GetDuplicatedArgs(string[] args)
        {
            var parsedFlagOptions = new HashSet<FlagOption>();

            var needParseArgs = GetNeedParseArgs(args);
            foreach (var arg in needParseArgs)
            {
                var flag = GetFlag(arg.Arg);
                var isDuplicated = flag != null && !parsedFlagOptions.Add(flag);
                if (isDuplicated)
                {
                    return arg.Trigger;
                }
            }
            return null;
        }
    }

    class ArgTrigger
    {
        public ArgTrigger(string arg, string trigger)
        {
            Arg = arg;
            Trigger = trigger;
        }

        public string Arg { get; }
        public string Trigger { get; }
    }
}