using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal enum PrioritizationTypeDataModel
{
    strawManRandom,
    sumOfCategoryMaximums,
    eventBased,
    bestObjectiveFunction
}

internal struct StoryDataModel
{
    internal int numTopScenesForUser;
    internal PrioritizationTypeDataModel? prioritizationType; //optional

    internal List<StoryNodeDataModel> nodes;
    internal StoryNodeDataModel? startingNode; //optional

    internal StoryStateDataModel initialStoryState;

    internal List<GlobalRuleDataModel>? globalRules; // optional
}