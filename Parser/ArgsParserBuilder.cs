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

        public ArgsParserBuilder()
        {
            argsParser = new ArgsParser();
        }

        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this, null);    
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