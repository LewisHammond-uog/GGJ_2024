extends Node

var main_scene : PackedScene = preload("res://Scenes/Main.tscn")
var main_menu : PackedScene = preload("res://Scenes/main_menu.tscn")

func _ready():
	EventBus.connect("go_level", _load_level)
	EventBus.connect("go_menu", _load_menu)

func _load_level():
	_kill_kids()
	var instance = main_scene.instantiate()
	add_child(instance)
	
func _load_menu():
	_kill_kids()
	var instance = main_menu.instantiate()
	add_child(instance)
	
func _kill_kids():
	var child = get_child(0)
	child.queue_free()
