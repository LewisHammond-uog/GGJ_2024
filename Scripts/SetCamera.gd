extends Node

@export var cam : Camera3D = get_parent()

# Called when the node enters the scene tree for the first time.
func _ready():
	cam.set_current(true)
	print(cam)
