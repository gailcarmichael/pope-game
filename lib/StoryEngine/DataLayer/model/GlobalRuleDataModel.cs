using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

public enum HowManyDataModel { all, any }
public enum ListTypeForNodesDataModel { tags, storyElements, nodeIDs }
public enum NodeStateDataModel { seen, notSeen }
public enum TagStateDataModel { present, notPresent }

public record GlobalRuleDataModel
{
    public string? ID { get; init; }
    public NodeFilterDataModel? NodeFilter { get; init; }
    public StoryStateFilterDataModel? StoryStateFilter { get; init; }
    public NodesAffectedDataModel? NodesAffected { get; init; }
}

public record NodeFilterDataModel(ListTypeForNodesDataModel CheckNodesUsing,
                                    List<string> ListToCheck,
                                    HowManyDataModel HowManyRequired,
                                    NodeStateDataModel NodeState)
{

}

public record StoryStateFilterDataModel(List<string> TagsToCheck, 
                                        HowManyDataModel HowManyRequired, 
                                        TagStateDataModel TagState)
{

}

public record NodesAffectedDataModel(ListTypeForNodesDataModel CheckNodesUsing,
                                        List<string> ListToCheck,
                                        HowManyDataModel HowManyRequired)
{

}