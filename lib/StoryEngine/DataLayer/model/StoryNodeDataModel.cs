using System.Collections.Generic;

namespace StoryEngine.StoryEngineDataModel;

internal enum NodeTypeDataModel
{
    kernel,
    satellite
}

internal struct StoryNodeDataModel
{
    internal string id;
    internal NodeTypeDataModel type;
    internal bool lastNode;

    internal string teaserText;
    internal string eventText;

    internal FunctionalDescriptionDataModel? functionalDescription; //optional

    internal PrerequisiteDataModel? prerequisite; //optional

    internal List<ChoiceDataModel>? choices; //optional

    internal StoryNodeDataModel(
            string new_id,
            NodeTypeDataModel new_type,
            string new_teaserText,
            string new_eventText,
            FunctionalDescriptionDataModel? new_funcDesc = null,
            PrerequisiteDataModel? new_prerequisite = null,
            List<ChoiceDataModel>? new_choices = null,
            bool new_lastNode = false)
    {
        id = new_id;
        type = new_type;
        teaserText = new_teaserText;
        eventText = new_eventText;
        functionalDescription = new_funcDesc;
        prerequisite = new_prerequisite;
        choices = new_choices;
    }
}