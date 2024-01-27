using Godot;
using System;

public partial class Weapon : Node
{
	public PlayerController Player;
	
	public WeaponResource Resource;
	public Timer FireTimer;
	public Node AnimationNode;

	public Weapon(WeaponResource resource)
	{
		Resource = resource;
		FireTimer = new Timer();
		FireTimer.Autostart = false;
		FireTimer.OneShot = true;
		FireTimer.Stop();
		AddChild(FireTimer);
		FireTimer.Timeout += SpawnBullet;
	}

	public void FireWeapon()
	{
		if (Resource.Animation != null)
		{
			AnimationNode = Resource.Animation.Instantiate();
			AddChild(AnimationNode);
		}
		if(Resource.WindUp > 0f)
			FireTimer.Start(Resource.WindUp);
		else
			SpawnBullet();
	}

	public void SpawnBullet()
	{
		var bullet = Resource.Bullet.Instantiate<BaseProjectile>();
		bullet.GlobalPosition = Player.GlobalPosition;
		bullet.Rotation = Player.Rotation;
		GetTree().Root.AddChild(bullet);
		bullet.Setup(-Player.Transform.Basis.Z);
	}
}
