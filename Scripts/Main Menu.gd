extends Control

func _ready():
	get_tree().paused = false

func _on_play_pressed():
	EventBus.go_level.emit()

func _on_fullscreen_toggled(button_pressed):
	if button_pressed:
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_MAXIMIZED)
	else: 
		DisplayServer.window_set_mode(DisplayServer.WINDOW_MODE_FULLSCREEN)

func _on_quit_pressed():
	get_tree().quit()
