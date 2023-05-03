using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record OutcomeDataModel(string Text)
{
    internal List<QuantifiableModifierDataModel>? QuantifiableModifiers { get; init; } //optional
    internal List <TagModifierDataModel>? TagModifiers {get; init; } //optional
}

internal record QuantifiableModifierDataModel(string ElementID, bool Absolute, int Delta)
{
}

internal enum TagActionDataModel
{
    add,
    remove
}

internal record TagModifierDataModel(string ElementID, TagActionDataModel Action)
{
}