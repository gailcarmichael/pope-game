using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public enum PrioritizationTypeDataModel
{
    strawManRandom,
    sumOfCategoryMaximums,
    eventBased,
    bestObjectiveFunction
}

public record StoryDataModel(
    int NumTopScenesForUser,
    List<StoryNodeDataModel> Nodes,
    StoryStateDataModel InitialStoryState
)
{
    public PrioritizationTypeDataModel? PrioritizationType { get; init; } //optional
    public StoryNodeDataModel? StartingNode { get; init; } //optional
    public List<GlobalRuleDataModel>? GlobalRules { get; init; } // optional
}