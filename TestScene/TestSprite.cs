using Godot;
using StoryEngine;
using StoryEngine.StoryEngineDataModel;


public class Logger : IStoryEngineLogger
{
    public void Write(object? content = null)
    {
		if (content is null) GD.Print("\n");
        else GD.Print(content?.ToString());
    }
}


public partial class TestSprite : Sprite2D
{
	private int Speed = 400;
	private float AngularSpeed = Mathf.Pi;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Set the logger
		StoryEngineAPI.Logger = new Logger();

        GD.Print("Test story JSON source:\n-----\n");
		GD.Print(StoryEngineAPI.GetTestStoryJSON());
        GD.Print("-----");

        GD.Print("\n\nTest element collection JSON source:\n-----\n");
        GD.Print(StoryEngineAPI.GetTestStoryElementCollectionJSON());
        GD.Print("-----\n");

		StoryEngineAPI storyEngine = new StoryEngineAPI(
			StoryEngineAPI.GetTestStoryJSON(),
			StoryEngineAPI.GetTestStoryElementCollectionJSON());

		GD.Print("\nStory is valid: " + storyEngine.IsStoryValid());
		GD.Print("\n\n");
		// GD.Print(storyEngine.CurrentNodeTeaserText());
		// GD.Print(storyEngine.CurrentNodeEventText());
		// GD.Print(storyEngine.CurrentNodeChoiceText(0));
        // GD.Print("\n\n");

        string story_json = storyEngine.StoryJSON();
		string element_col_json = storyEngine.StoryElementCollectionJSON();
		
        
        // GD.Print("Story JSON:\n-----\n");
		// GD.Print(story_json);
        // GD.Print("-----\n");
		StoryDataModel? storyDataModel = StoryEngineAPI.DeserializeStoryFromJSON(story_json);

        
		// GD.Print("\n\n");
        // GD.Print("Element Collection JSON:\n-----\n");
        // GD.Print(element_col_json);
        // GD.Print("-----\n");
        StoryElementCollectionDataModel? col = StoryEngineAPI.DeserializeStoryElementCollectionFromJSON(element_col_json);
        //TODO: check element col round trip
    }
}
