extends AudioStreamPlayer3D

var active

func _input(event):
	if Input.is_action_pressed("movement_down") || Input.is_action_pressed("movement_up") || Input.is_action_pressed("movement_left") || Input.is_action_pressed("movement_right"):
		if !active:
			play()
			active = true
			await get_tree().create_timer(0.2).timeout
			active = false
	else:
		stop()
		
	if Input.is_action_pressed("movement_sprint"):
		pitch_scale = 1.5
	else:
		pitch_scale = 1
