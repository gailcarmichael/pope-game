using Godot;
using System;
using StoryEngine;

public partial class MyCSharpNode : Node
{
	public string TeaserText() {
		StoryEngineAPI storyEngine = new StoryEngineAPI("filename", "otherFilename");
		return storyEngine.CurrentNodeTeaserText();
	}
}
