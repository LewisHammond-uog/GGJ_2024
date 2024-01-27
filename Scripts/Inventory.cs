using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
	public List<Weapon> Weapons;

	public Weapon CurrentWeapon;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Weapons.Add(new Weapon(GD.Load<WeaponResource>("res://Weapons/Punch.tres")));
		CurrentWeapon = Weapons[0];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
