extends MeshInstance3D

var current_amount
var raise_amount = -2
var lower_amount = +2

var raised : bool = false:
	set(value): 
		raised = value
		if raised:
			current_amount = raise_amount
		if !raised:
			current_amount = lower_amount

func _ready():
	current_amount = lower_amount

func _input(event):
	if Input.is_action_just_pressed("ui_accept"):
		_door_toggle()
		
func _door_toggle():
		var target = position
		target.y += current_amount
		var tween = create_tween().set_trans(Tween.TRANS_ELASTIC)
		tween.tween_property(self, "position", target, 1)
		raised = !raised
