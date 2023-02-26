extends Control

@onready var text = get_parent().get_node("Node").dialogue_1

var dialogue_index = 0
var finished
var active

var render_position
var expression
var tween

func _ready():
	load_dialogue()
	
func _physics_process(delta):
	if active:
		if Input.is_action_just_pressed("ui_accept"):
			if finished == true:
				load_dialogue()
			else:
				$DialogueContainer/RichTextLabel.visible_ratio = 1
				finished = true
		if $DialogueContainer/Label.text == "Johanne":
			$SpaceCutieMeEvolved.visible = true
			if render_position == "left_pos":
				$SpaceCutieMeEvolved.global_position = get_parent().get_node("left_pos").position
		if $Button.text == "":
			$Button.visible = false
		else: 
			$Button.visible = true
			
		if $Button2.text == "":
			$Button2.visible = false
		else: 
			$Button2.visible = true

func load_dialogue():
	if dialogue_index < text.size():
		active = true
		finished = false
		
		$DialogueContainer.visible = true
		$DialogueContainer/RichTextLabel.text = text[dialogue_index]["Text"]
		$DialogueContainer/Label.text = text[dialogue_index]["Name"]
		$Button.text = text[dialogue_index]["Choices"][0]
		$Button2.text = text[dialogue_index]["Choices"][1]
		
		render_position = text[dialogue_index]["Name"]
		
		$DialogueContainer/RichTextLabel.visible_ratio = 0
		# print(text[dialogue_index]["Text"])
		tween = get_tree().create_tween()
		tween.tween_property($DialogueContainer/RichTextLabel, "visible_ratio", 1, 2).from(0).set_ease(2).set_trans(0)
		
	else:
		$DialogueContainer.visible = false
		finished = true
		active = false
	dialogue_index += 1
	

func _on_Button_pressed():
	if $Button.text == "Hi there":
		$Button.text = ""
		$Button2.text = ""
		text = get_parent().get_node("Node").after_choice_1
		dialogue_index = 0
		load_dialogue()

func _on_Button2_pressed():
	if $Button2.text == "Who are you?":
		$Button.text = ""
		$Button2.text = ""
		text = get_parent().get_node("Node").after_choice_2
		dialogue_index = 0
		load_dialogue()

func _on_Tween_finished():
	finished = true
