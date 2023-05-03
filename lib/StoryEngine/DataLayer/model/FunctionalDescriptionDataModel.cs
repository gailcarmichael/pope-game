using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record FunctionalDescriptionDataModel()
{
    // These are optional, but logically it makes sense to have at least one of the two present
    public Dictionary<string, int>? ElementProminences { get; init; } //optional
    public List<string>? TaggableElementIDs { get; init; } //optional
}