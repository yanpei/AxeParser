using System;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// To build a agrs builder
    /// </summary>
    public class ArgsParserBuilder
    {
        CommandBuilder defaultCommandBuilder;

        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this, null);    
        }

        /// <returns>Parser</returns>
        public ArgsParser Build()
        {
            return defaultCommandBuilder.parser;
        }

        internal void SetDefaultCommand(CommandBuilder commandBuilder)
        {
            if (defaultCommandBuilder != null)
            {
                throw new InvalidOperationException("Default Command is arleady defined");
            }

            defaultCommandBuilder = commandBuilder;
        }
    }
}