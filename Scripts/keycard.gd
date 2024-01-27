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

@export var security_level : Globals.SecurityLevel

func _ready():
	_assign_strip_colour()
	$AnimationPlayer.play("bob_and_spin")

func _assign_strip_colour():
	var chosen_mat : Material
	chosen_mat = strip_mats[security_level]
	$MeshInstance3D.set_surface_override_material(0, chosen_mat)

func _on_area_3d_body_entered(body):
	if security_level > Globals.PlayerLevel:
		Globals.PlayerLevel = security_level 
	queue_free()
