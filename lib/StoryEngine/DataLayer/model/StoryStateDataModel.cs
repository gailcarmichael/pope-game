using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public struct StoryStateDataModel
{
    public Dictionary<string, float> elementValues;
    public Dictionary<string, float> elementDesires;
    public List<string> tagList;
}