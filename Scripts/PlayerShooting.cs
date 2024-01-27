using Godot;
using System;

public partial class PlayerShooting : Node
{
	[Export] private Inventory inventory;

	private Timer shootTimer;
	private bool canShoot = true;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		shootTimer = new Timer();
		shootTimer.Autostart = false;
		shootTimer.OneShot = true;
		shootTimer.Stop();
		AddChild(shootTimer);
		shootTimer.Timeout += ResetCanShoot;
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
	}

	public void ResetCanShoot()
	{
		canShoot = true;
	}
}
