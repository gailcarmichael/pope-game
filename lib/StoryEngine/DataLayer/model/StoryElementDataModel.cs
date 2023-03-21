namespace StoryEngine.StoryEngineDataModel;

internal enum ElementTypeDataModel
{
    quantifiable, // can attach a number to it
    quantifiableStoryStateOnly, // can only be used in the story state (not with nodes)
    taggable, // can tag nodes or the story state with it
}

internal struct StoryElementDataModel
{
    internal string id;
    internal string category;
    internal string name;
    internal ElementTypeDataModel type;

    internal StoryElementDataModel(string new_id, string new_category, string new_name, ElementTypeDataModel new_type)
    {
        id = new_id;
        category = new_category;
        name = new_name;
        type = new_type;
    }
}