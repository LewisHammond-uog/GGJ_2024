extends MeshInstance3D

var opening :bool = false
var queue : int = 0

var current_amount
var raise_amount = -3
var lower_amount = raise_amount * -1

@export var locked : bool = false
@export var security_level : Globals.SecurityLevel

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
	return
	if Input.is_action_just_pressed("ui_accept"):
		_door_toggle(!raised)
	
func _door_toggle(desired_state):
	_security_check()
	if locked || desired_state == raised:
		return
	
	if opening:
		await get_tree().create_timer(1).timeout
	
	var target = position
	target.y += current_amount
	opening = true
	
	var tween = create_tween().set_trans(Tween.TRANS_ELASTIC)
	tween.tween_property(self, "position", target, 1)
	
	await tween.finished
	opening = false
	if raised:
		raised = false
	else:
		raised = true

func _security_check():
	if Globals.PlayerLevel >= security_level:
		locked = false
	else:
		locked = true
	
func _on_area_3d_body_entered(body):
	print("hi")
	_door_toggle(true)

func _on_area_3d_body_exited(body):
	print("bye")
	_door_toggle(false)
