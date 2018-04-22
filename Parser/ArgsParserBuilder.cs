using System;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// To build a agrs builder
    /// </summary>
    public class ArgsParserBuilder
    {
        readonly ArgsParser argsParser;

        /// <summary>
        /// init args parser
        /// </summary>
        public ArgsParserBuilder()
        {
            argsParser = new ArgsParser();
        }

        /// <summary>
        /// return a new default comman
        /// </summary>
        /// <returns></returns>
        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this, null, string.Empty);    
        }

        /// <returns>Parser</returns>
        public ArgsParser Build()
        {
            return argsParser;
        }

        internal void SetDefaultCommand(CommandBuilder commandBuilder)
        {
            if (argsParser.DefaultCommand != null)
            {
                throw new InvalidOperationException("Default Command is arleady defined");
            }

            argsParser.DefaultCommand = commandBuilder.commandDefination;
        }
    }
}