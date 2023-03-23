extends Node


# var storynode = storyengine.new()

# Called when the node enters the scene tree for the first time.
func _ready():
	# This code basically lifted from https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html
	var my_csharp_script = load("res://MyCSharpNode.cs") # needs to have the same name as the class !
	var my_csharp_node = my_csharp_script.new()
	var text_from_csharp = my_csharp_node.TeaserText()
	print(text_from_csharp) # outputs "node 2 teaser" 


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	pass
