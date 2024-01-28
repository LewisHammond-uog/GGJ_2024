extends Label3D

var option : String
var count : int
@export var dialogue : Array[String]

func _ready():
	count = 0
	option = dialogue[count]
	text = option

func _on_timer_timeout():
	if option < dialogue.max():
		count += 1
	else:
		count = 0
	option = dialogue[count]
	text = option


func _on_area_3d_body_entered(body):
	EventBus.go_menu.emit()
