using Godot;
using System;
using StoryEngine;

public partial class MyCSharpNode : Node
{
	private StoryEngineAPI	storyEngine = new StoryEngineAPI("filename", "otherFileName");
	
	public string TeaserText() {
		return storyEngine.CurrentNodeTeaserText();
	}
}
