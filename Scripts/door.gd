extends Node3D

var opening : bool = false
var want_open : bool = false

var open_time : float = 1.0

var raise_amount = 3
var lower_amount = raise_amount * -1

@export var locked : bool = false
@export var security_level : Globals.SecurityLevel

var raised : bool = false
	
func _door_toggle():
	_security_check()
	if locked:
		return
	
	if want_open:
		var target = position
		target.y += raise_amount
		_move_door(target)

	if !want_open:
		var target = position
		target.y += lower_amount
		_move_door(target)

func _move_door(target):
	opening = true
	var tween = create_tween().set_trans(Tween.TRANS_ELASTIC)
	tween.tween_property(self, "position", target, open_time)
	await tween.finished
	opening = false

func _security_check():
	if Globals.PlayerLevel >= security_level:
		locked = false
	else:
		locked = true
	
func _on_area_3d_body_entered(body):
	if raised:
		return
	if opening:
		await get_tree().create_timer(open_time + 0.2).timeout
	raised = true
	want_open = true
	_door_toggle()

func _on_area_3d_body_exited(body):
	if !raised:
		return
	if opening:
		await get_tree().create_timer(open_time + 0.2).timeout
	raised = false
	want_open = false
	_door_toggle()
