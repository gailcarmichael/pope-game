namespace StoryEngine.StoryEngineDataModel;

public enum ElementTypeDataModel
{
    quantifiable, // can attach a number to it
    quantifiableStoryStateOnly, // can only be used in the story state (not with nodes)
    taggable, // can tag nodes or the story state with it
}

public record StoryElementDataModel(string ID, string Category, string Name, ElementTypeDataModel Type)
{

}