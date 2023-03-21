using System.Collections.Generic;

namespace StoryEngineDataModel;

public enum HowMany { all, any }
public enum ListTypeForNodes { tags, storyElements, nodeIDs }
public enum NodeState { seen, notSeen }
public enum TagState { present, notPresent }

public struct GlobalRule
{
    public string? id;
    public NodeFilter? nodeFilter;
    public StoryStateFilter? storyStateFilter;
    public NodesAffected? nodesAffected;
}

public struct NodeFilter
{
    public ListTypeForNodes checkNodesUsing;
    public List<string> listToCheck;
    public HowMany howManyRequired;
    public NodeState nodeState;
}

public struct StoryStateFilter
{
    public List<string> tagsToCheck;
    public HowMany howManyRequired;
    public TagState tagState;
}

public struct NodesAffected
{
    public ListTypeForNodes checkNodesUsing;
    public List<string> listToCheck;
    public HowMany howManyRequired;
}