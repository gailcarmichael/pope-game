using System.Collections.Generic;

namespace StoryEngineDataModel;

internal enum NodeType
{
    kernel,
    satellite
}

internal struct StoryNode
{
    internal string id;
    internal NodeType type;
    internal bool lastNode;

    internal string teaserText;
    internal string eventText;

    internal FunctionalDescription? functionalDescription; //optional

    internal Prerequisite? prerequisite; //optional

    internal List<ChoiceData>? choices; //optional
}