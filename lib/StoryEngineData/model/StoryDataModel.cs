using System.Collections.Generic;

namespace StoryEngineDataModel;

internal enum PrioritizationType
{
    strawManRandom,
    sumOfCategoryMaximums,
    eventBased,
    bestObjectiveFunction
}

internal struct Story
{
    internal int numTopScenesForUser;
    internal PrioritizationType? prioritizationType; //optional

    internal List<StoryNode> nodes;
    internal StoryNode? startingNode; //optional

    internal StoryState initialStoryState;

    internal List<GlobalRule>? globalRules; // optional
}