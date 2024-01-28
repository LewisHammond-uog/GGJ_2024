extends Node

enum SecurityLevel {
	None,
	Green,
	Red, 
	Blue,
	Purple,
	Gold
}

var PlayerLevel : SecurityLevel = 0

var Player:Node3D
