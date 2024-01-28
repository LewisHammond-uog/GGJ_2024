extends Node

enum SecurityLevel {
	None,
	Green,
	Red, 
	Blue,
	Purple,
	Gold
}

var PlayerLevel : SecurityLevel = 0:
	set(value):
		PlayerLevel = value
		EventBus.security_change.emit(PlayerLevel)

var Player:Node3D
