using System.Text.RegularExpressions;

namespace Parser
{
    static class FlagRegex
    {
        public static readonly Regex FullFormRegex = new Regex("^[A-Za-z0-9_]([A-Za-z0-9_-])*$");
        public static readonly Regex AbbrevationFormRegex = new Regex("^[A-Za-z]$");
    }
}
