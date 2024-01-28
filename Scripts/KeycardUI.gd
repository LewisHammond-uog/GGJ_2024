extends GridContainer


# Called when the node enters the scene tree for the first time.
func _ready():
	EventBus.connect("security_change", _update_display)


func _update_display(num):
	get_child(num - 1).show()
