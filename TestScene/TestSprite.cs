using Godot;
using System;
using StoryEngine;
using StoryEngine.StoryEngineDataModel;

public partial class TestSprite : Sprite2D
{
	private int Speed = 400;
	private float AngularSpeed = Mathf.Pi;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StoryEngineAPI storyEngine = new StoryEngineAPI(
			StoryEngineAPI.GetTestStoryJSON(),
			StoryEngineAPI.GetTestStoryElementCollectionJSON());

		GD.Print("Story is valid: " + storyEngine.IsStoryValid());
		
		GD.Print("\n\n");
		GD.Print(storyEngine.Story?.ToString()); // TODO implement custom ToString

		GD.Print("\n\n");
		GD.Print(storyEngine.CurrentNodeTeaserText());
		GD.Print(storyEngine.CurrentNodeEventText());
		GD.Print(storyEngine.CurrentNodeChoiceText(0));

        string story_json = StoryEngineAPI.GetTestStoryJSON();
		string element_col_json = StoryEngineAPI.GetTestStoryElementCollectionJSON();
		
        GD.Print("\n\n");
        GD.Print("Story JSON:\n");
		GD.Print(story_json);
		StoryDataModel? story = StoryEngineAPI.DeserializeStoryFromJSON(story_json);
		

		//TODO: check story round trip
        
		GD.Print("\n\n");
        GD.Print("Element Collection JSON:\n");
        GD.Print(element_col_json);
        StoryElementCollectionDataModel? col = StoryEngineAPI.DeserializeStoryElementCollectionFromJSON(element_col_json);
        //TODO: check element col round trip
    }
}
