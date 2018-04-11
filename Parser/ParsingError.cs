namespace Parser
{
    /// <summary>
    ///
    /// </summary>
    public class ParsingError
    {
        /// <value>Error code.</value>
        public ParsingErrorCode Code { get; }
        /// <value>parsing error trigger reason.</value>
        public string Trigger { get; }

        /// <summary>
        /// Initialize a ParsingError
        /// </summary>
        /// <param name="code">Error code</param>
        /// <param name="trigger">Trigger reason.</param>
        public ParsingError(ParsingErrorCode code, string trigger)
        {
            Code = code;
            Trigger = trigger;
        }
    }

    /// <summary>
    /// ParsingErrorCode
    /// </summary>
    public enum ParsingErrorCode
    {
        /// <summary>
        /// FreeValueNotSupported
        /// </summary>
        FreeValueNotSupported = 0,
        /// <summary>
        /// DuplicateFlagsInArgs
        /// </summary>
        DuplicateFlagsInArgs = 1
    }
}