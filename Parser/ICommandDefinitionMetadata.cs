﻿namespace Parser
{
    public interface ICommandDefinitionMetadata
    {
        string Symbol { get; }
        IOptionDefinitionMetadata[] GetRegisteredOptionsMetadata();
    }
}