using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal struct OutcomeDataModel
{
    internal string text;
    
    internal List<QuantifiableModifierDataModel>? quantifiableModifiers; //optional
    internal List <TagModifierDataModel>? tagModifiers; //optional

    public OutcomeDataModel(string new_text)
    {
        text = new_text;
    }
}

internal struct QuantifiableModifierDataModel
{
    internal string elementID;
    internal bool absolute;
    internal int delta;

    internal QuantifiableModifierDataModel(string new_elementID, bool new_absolute, int new_delta)
    {
        elementID = new_elementID;
        absolute = new_absolute;
        delta = new_delta;
    }
}

internal enum TagActionDataModel
{
    add,
    remove
}

internal struct TagModifierDataModel
{
    internal string elementID;
    internal TagActionDataModel action;

    internal TagModifierDataModel(string new_elementID, TagActionDataModel new_action)
    {
        elementID = new_elementID;
        action = new_action;
    }
}