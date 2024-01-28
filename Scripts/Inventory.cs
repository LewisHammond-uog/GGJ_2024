using Godot;
using System;
using System.Collections.Generic;

public partial class Inventory : Node
{
	[Export] public PlayerController Player;
	[Export] public Camera3D Camera3D;
	public List<Weapon> Weapons = new List<Weapon>();

	public Weapon CurrentWeapon;
	public EventHandler<InventoryItemArgs> ItemAddedEvent;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Weapon fists = new Weapon(GD.Load<WeaponResource>("res://Weapons/Punch.tres"));
		fists.Player = Player;
		fists.Camera3D = Camera3D;
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

	public void AddWeaponToInventory(Weapon weaponToAdd)
	{
		weaponToAdd.Player = Player;
		weaponToAdd.Camera3D = Camera3D;
		Reparent(weaponToAdd);
		Weapons.Add(weaponToAdd);
		ItemAddedEvent?.Invoke(this, new InventoryItemArgs()
		{
			item = weaponToAdd
		});
	}

	public void AddAmmo(Weapon weapon)
	{
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}

public class InventoryItemArgs : EventArgs
{
	public Weapon item;
}
