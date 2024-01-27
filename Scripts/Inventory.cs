using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
	[Export] private PlayerController Player;
	public List<Weapon> Weapons = new List<Weapon>();

	public Weapon CurrentWeapon;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Weapon fists = new Weapon(GD.Load<WeaponResource>("res://Weapons/Punch.tres"));
		fists.Player = Player;
		AddChild(fists);
		Weapons.Add(fists);
		CurrentWeapon = Weapons[0];
	}

	public void SwapCurrentWeapon(int index)
	{
		CurrentWeapon.FireTimer.Stop();
		CurrentWeapon.AnimationNode.QueueFree();

		CurrentWeapon = Weapons[index];
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
