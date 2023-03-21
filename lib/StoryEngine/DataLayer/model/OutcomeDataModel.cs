using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record OutcomeDataModel(string Text)
{
    internal List<QuantifiableModifierDataModel>? QuantifiableModifiers { get; init; } //optional
    internal List <TagModifierDataModel>? TagModifiers {get; init; } //optional
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