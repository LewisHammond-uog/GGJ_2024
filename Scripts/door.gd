extends Node3D

var mat_black : Material = preload("res://Enviroment/Objects/Materials/strips/strip_black.tres")
var mat_green : Material = preload("res://Enviroment/Objects/Materials/strips/strip_green.tres")
var mat_blue : Material = preload("res://Enviroment/Objects/Materials/strips/strip_blue.tres")
var mat_red : Material = preload("res://Enviroment/Objects/Materials/strips/strip_red.tres")
var mat_purple : Material = preload("res://Enviroment/Objects/Materials/strips/strip_purple.tres")
var mat_gold : Material = preload("res://Enviroment/Objects/Materials/strips/strip_gold.tres")

var strip_mats = [
	mat_black,
	mat_green,
	mat_red, 
	mat_blue,
	mat_purple,
	mat_gold
]

var opening : bool = false
var want_open : bool = false

var open_time : float = 0.95

var raise_amount = 3
var lower_amount = raise_amount * -1

@export var locked : bool = false
@export var security_level : Globals.SecurityLevel

var raised : bool = false
	
func _ready():
	_assign_strip_colour()
	
func _door_toggle():
	_security_check()
	if locked:
		return
	
	if want_open:
		var target = $Door.position
		target.y += raise_amount
		_move_door(target)

	if !want_open:
		var target = $Door.position
		target.y += lower_amount
		_move_door(target)

func _move_door(target):
	opening = true
	var tween = create_tween().set_trans(Tween.TRANS_ELASTIC)
	tween.tween_property($Door, "position", target, open_time)
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

func _assign_strip_colour():
	var chosen_mat : Material
	chosen_mat = strip_mats[security_level]
	%"Colour Strip".set_surface_override_material(0, chosen_mat)
	pass
