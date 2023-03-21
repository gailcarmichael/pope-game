using System.Collections.Generic;

namespace StoryEngineDataModel;

public enum PrioritizationType
{
    strawManRandom,
    sumOfCategoryMaximums,
    eventBased,
    bestObjectiveFunction
}

public struct Story
{
    public int numTopScenesForUser;
    public PrioritizationType? prioritizationType; //optional

    public List<StoryNode> nodes;
    public StoryNode? startingNode; //optional

    public StoryState initialStoryState;

    public List<GlobalRule>? globalRules; // optional
}