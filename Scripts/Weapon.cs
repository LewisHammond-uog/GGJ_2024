using Godot;
using System;

public partial class Weapon : Node
{
	public WeaponResource Resource;

	public Weapon(WeaponResource resource)
	{
		Resource = resource;
	}

	public void FireWeapon()
	{
		
	}
}
