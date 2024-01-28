extends AudioStreamPlayer


func _input(event):
	if Input.is_action_just_pressed("kick_fire"):
		play()
