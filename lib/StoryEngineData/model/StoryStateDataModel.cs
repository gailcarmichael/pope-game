using System.Collections.Generic;

namespace StoryEngineDataModel;

internal struct StoryState
{
    internal Dictionary<string, float> elementValues;
    internal Dictionary<string, float> elementDesires;
    internal List<string> tagList;
}