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
			"{}");

		GD.Print("Story is valid: " + storyEngine.IsStoryValid());
		
		GD.Print("\n\n");
		GD.Print(storyEngine.Story?.ToString()); // TODO implement custom ToString

		GD.Print("\n\n");
		GD.Print(storyEngine.CurrentNodeTeaserText());
		GD.Print(storyEngine.CurrentNodeEventText());
		GD.Print(storyEngine.CurrentNodeChoiceText(0));

        GD.Print("\n\n");
        string json = StoryEngineAPI.GetTestStoryJSON();
        GD.Print(json);
		StoryDataModel? story = StoryEngineAPI.DeserializeStoryFromJSON(json);
        GD.Print("\n\n");
		//if (story != null) GD.Print(StoryEngineAPI.SerializeStoryToJSON(story));
	}
}
