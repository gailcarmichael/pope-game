namespace StoryEngineDataModel;

internal enum ElementType
{
    quantifiable, // can attach a number to it
    quantifiableStoryStateOnly, // can only be used in the story state (not with nodes)
    taggable, // can tag nodes or the story state with it
}

internal struct StoryElement
{
    internal string id;
    internal string category;
    internal string name;
    internal ElementType type;
}