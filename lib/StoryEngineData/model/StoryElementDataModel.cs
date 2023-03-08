namespace StoryEngineDataModel;

public enum ElementType
{
    quantifiable, // can attach a number to it
    quantifiableStoryStateOnly, // can only be used in the story state (not with nodes)
    taggable, // can tag nodes or the story state with it
}

public struct StoryElement
{
    public string id;
    public string category;
    public string name;
    public ElementType type;
}