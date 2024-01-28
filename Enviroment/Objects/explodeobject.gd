extends Node3D
@export var parentNode:Node3D = null
@export_flags_3d_physics var fragment_collision_layer:int = 3
@export_flags_3d_physics var fragment_collision_mask:int = 3
@export var explosion_speed:float = 0.4
@export var min_flag_lifetime:float = 0.8
@export var max_flag_lifetime:float = 1.8

# Called when the node enters the scene tree for the first time.
func _ready():
	pass # Replace with function body.
	

# Called every frame. 'delta' is the elapsed time since the previous frame.
func _process(delta):
	pass
	#if Input.is_action_just_pressed("movement_jump"):
	#	explode()

func explode():
	var player = Globals.Player
	parentNode.queue_free()
	
	for child in parentNode.$fractured.get_children():
		if child is MeshInstance3D:
			var frag:Fragment = preload("res://Scenes/fragment.tscn").instantiate()
			frag.init_from_mesh(child)
			frag.collision_layer = fragment_collision_layer
			frag.collision_mask = fragment_collision_mask
			get_tree().root.add_child(frag)
			
			# to future person, need to make it fly away from the player.....
			var vel:Vector3 = (frag.global_transform.origin - player.global_transform.origin) * explosion_speed
			frag.linear_velocity = vel
			
			frag.lifetime = randf_range(min_flag_lifetime, max_flag_lifetime)
		
