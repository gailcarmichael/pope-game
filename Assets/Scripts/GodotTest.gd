extends Node

var my_csharp_script = load("res://Assets/Scripts/MyCSharpNode.cs") # needs to have the same name as the class!

# Called when the node enters the scene tree for the first time.
func _ready():
	# This code basically lifted from https://docs.godotengine.org/en/stable/tutorials/scripting/cross_language_scripting.html

	var my_csharp_node = my_csharp_script.new()
	var text_from_csharp = my_csharp_node.TeaserText()
	print("Ready(): " + text_from_csharp) # outputs "node 2 teaser" 


# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(_delta):
	var my_csharp_node = my_csharp_script.new()
	var text_from_csharp = my_csharp_node.TeaserText()
	get_node("DialogBox/DialogText").text = "Process(): " + text_from_csharp
	
	pass
