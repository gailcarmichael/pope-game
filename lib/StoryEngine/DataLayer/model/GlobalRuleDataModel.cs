using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal enum HowManyDataModel { all, any }
internal enum ListTypeForNodesDataModel { tags, storyElements, nodeIDs }
internal enum NodeStateDataModel { seen, notSeen }
internal enum TagStateDataModel { present, notPresent }

internal struct GlobalRuleDataModel
{
    internal string? id;
    internal NodeFilterDataModel? nodeFilter;
    internal StoryStateFilterDataModel? storyStateFilter;
    internal NodesAffectedDataModel? nodesAffected;
}

internal struct NodeFilterDataModel
{
    internal ListTypeForNodesDataModel checkNodesUsing;
    internal List<string> listToCheck;
    internal HowManyDataModel howManyRequired;
    internal NodeStateDataModel nodeState;
}

internal struct StoryStateFilterDataModel
{
    internal List<string> tagsToCheck;
    internal HowManyDataModel howManyRequired;
    internal TagStateDataModel tagState;
}

internal struct NodesAffectedDataModel
{
    internal ListTypeForNodesDataModel checkNodesUsing;
    internal List<string> listToCheck;
    internal HowManyDataModel howManyRequired;
}