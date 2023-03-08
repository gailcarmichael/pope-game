using System.Collections.Generic;

namespace StoryEngineDataModel;

public enum NodeType
{
    kernel,
    satellite
}

public struct StoryNode
{
    public string id;
    public NodeType type;
    public bool lastNode;

    public string teaserText;
    public string eventText;

    public FunctionalDescription? functionalDescription; //optional

    public Prerequisite? prerequisite; //optional

    public List<ChoiceData>? choices; //optional
}