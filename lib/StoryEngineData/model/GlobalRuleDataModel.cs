using System.Collections.Generic;

namespace StoryEngineDataModel;

internal enum HowMany { all, any }
internal enum ListTypeForNodes { tags, storyElements, nodeIDs }
internal enum NodeState { seen, notSeen }
internal enum TagState { present, notPresent }

internal struct GlobalRule
{
    internal string? id;
    internal NodeFilter? nodeFilter;
    internal StoryStateFilter? storyStateFilter;
    internal NodesAffected? nodesAffected;
}

internal struct NodeFilter
{
    internal ListTypeForNodes checkNodesUsing;
    internal List<string> listToCheck;
    internal HowMany howManyRequired;
    internal NodeState nodeState;
}

internal struct StoryStateFilter
{
    internal List<string> tagsToCheck;
    internal HowMany howManyRequired;
    internal TagState tagState;
}

internal struct NodesAffected
{
    internal ListTypeForNodes checkNodesUsing;
    internal List<string> listToCheck;
    internal HowMany howManyRequired;
}