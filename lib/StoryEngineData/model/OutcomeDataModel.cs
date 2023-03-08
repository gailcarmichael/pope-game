using System.Collections.Generic;

namespace StoryEngineDataModel;

public struct Outcome
{
    public string text;
    
    public List<QuantifiableModifier>? quantifiableModifier; //optional
    public List <TagModifier>? tagModifier; //optional
}

public struct QuantifiableModifier
{
    public string elementID;
    public bool absolute;
    public int delta;
}

public enum TagAction
{
    add,
    remove
}

public struct TagModifier
{
    public string elementID;
    public TagAction action;
}