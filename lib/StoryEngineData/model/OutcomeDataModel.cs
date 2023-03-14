using System.Collections.Generic;

namespace StoryEngineDataModel;

internal struct Outcome
{
    internal string text;
    
    internal List<QuantifiableModifier>? quantifiableModifier; //optional
    internal List <TagModifier>? tagModifier; //optional
}

internal struct QuantifiableModifier
{
    internal string elementID;
    internal bool absolute;
    internal int delta;
}

internal enum TagAction
{
    add,
    remove
}

internal struct TagModifier
{
    internal string elementID;
    internal TagAction action;
}