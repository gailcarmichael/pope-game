using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public record StoryStateDataModel(
    Dictionary<string, float> ElementValues, 
    Dictionary<string, float> ElementDesires, 
    List<string> TagList)
{

}