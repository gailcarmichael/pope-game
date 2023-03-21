using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal struct FunctionalDescriptionDataModel
{
    internal Dictionary<string, int> elementProminences;
    internal List<string>? taggableElementIDs; //optional
}