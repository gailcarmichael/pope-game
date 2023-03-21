using System.Collections.Generic;

namespace StoryEngineDataModel;

public struct StoryState
{
    public Dictionary<string, float> elementValues;
    public Dictionary<string, float> elementDesires;
    public List<string> tagList;
}