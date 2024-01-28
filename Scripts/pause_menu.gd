extends Control

var game_paused : bool = false:
	set(value):
		game_paused = value
		get_tree().paused = game_paused
		if(game_paused):
			show()
			Input.mouse_mode = Input.MOUSE_MODE_CONFINED
		else:
			hide()
			Input.mouse_mode = Input.MOUSE_MODE_CAPTURED

func _ready():
	hide()

func _input(event : InputEvent):
	if(event.is_action_pressed("pause")):
		game_paused = !game_paused
	
func _on_resume_pressed():
	game_paused = !game_paused

func _on_quit_pressed():
	get_tree().quit()

func _on_toggle_fullscreen_toggled(button_pressed):
	if button_pressed:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_MAXIMIZED)
	else: 
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)

func _on_exit_to_menu_pressed():
	EventBus.go_menu.emit()
