using System;
using System.Linq;

namespace Parser
{
    /// <summary>
    /// To build a agrs builder
    /// </summary>
    public class ArgsParserBuilder
    {
        internal CommandBuilder DefaultCommandBuilder { get; set; }

        public CommandBuilder BeginDefaultCommand()
        {
            return new CommandBuilder(this);    
        }
        

        /// <returns>Parser</returns>
        public ArgsParser Build()
        {
            return DefaultCommandBuilder.parser;
        }
    }
}