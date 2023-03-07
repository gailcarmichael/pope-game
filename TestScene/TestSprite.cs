using Godot;
using System;
using StoryEngine;

public partial class TestSprite : Sprite2D
{
	private int Speed = 400;
	private float AngularSpeed = Mathf.Pi;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		StoryEngineAPI storyEngine = new StoryEngineAPI("filename", "otherFilename");

		// TODO: Directly accessing Story should not be allowed in the future; for now,
		// it's just for testing until we find a better way to control access and print
		// values generically from the library

		GD.Print("Story is valid: " + storyEngine.IsStoryValid());
		
		GD.Print("\n\n");
		GD.Print(storyEngine.Story.ToString()); // TODO implement custom ToString

		GD.Print("\n\n");
		GD.Print(storyEngine.CurrentNodeTeaserText());
		GD.Print(storyEngine.CurrentNodeEventText());
		GD.Print(storyEngine.CurrentNodeChoiceText(0));
	}
}
