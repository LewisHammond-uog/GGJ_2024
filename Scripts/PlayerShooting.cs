using Godot;
using System;

public partial class PlayerShooting : Node
{
	[Export] private Inventory inventory;

	private Timer shootTimer;
	private bool canShoot = true;
	
	private Timer kickTimer;
	private bool canKick = true;

	private Weapon Kick;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Kick = new Weapon(GD.Load<WeaponResource>("res://Weapons/Kick.tres"));
		Kick.Player = inventory.Player;
		Kick.Camera3D = inventory.Camera3D;
		AddChild(Kick);
		
		shootTimer = new Timer();
		shootTimer.Autostart = false;
		shootTimer.OneShot = true;
		shootTimer.Stop();
		AddChild(shootTimer);
		shootTimer.Timeout += ResetCanShoot;
		
		kickTimer = new Timer();
		kickTimer.Autostart = false;
		kickTimer.OneShot = true;
		kickTimer.Stop();
		AddChild(kickTimer);
		kickTimer.Timeout += ResetCanKick;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Input.IsActionPressed("primary_fire") && canShoot)
		{
			canShoot = false;
			inventory.CurrentWeapon.FireWeapon();
			shootTimer.Start(inventory.CurrentWeapon.Resource.Cooldown);
		}

		if (Input.IsActionPressed("kick_fire") && canKick)
		{
			DoKick();
		}
	}

	public void DoKick()
	{
		canKick = false;
		inventory.Player.OnKick();
		Kick.FireWeapon();
		kickTimer.Start(Kick.Resource.Cooldown);
	}

	public void ResetCanShoot()
	{
		canShoot = true;
	}
	
	public void ResetCanKick()
	{
		canKick = true;
	}
}
