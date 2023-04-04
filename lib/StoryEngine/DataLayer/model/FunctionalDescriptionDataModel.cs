using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record FunctionalDescriptionDataModel(Dictionary<string, int> ElementProminences)
{
    public List<string>? TaggableElementIDs { get; init; } //optional
}