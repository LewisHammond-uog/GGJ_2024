using Godot;
using System;
using GGJ24;

public partial class Weapon : Node
{
	public PlayerController Player;
	public Camera3D Camera3D;
	
	public WeaponResource Resource;
	public Timer FireTimer;
	public Node3D AnimationNode;

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
			AnimationNode = Resource.Animation.Instantiate<Node3D>();
			AnimationNode.TryFindNodeOfTypeInChildren<AnimatedSprite3D>().AnimationFinished += () => AnimationNode.QueueFree();
			Camera3D.AddChild(AnimationNode);
		}
		if(Resource.WindUp > 0f)
			FireTimer.Start(Resource.WindUp);
		else
			SpawnBullet();
	}

	public void SpawnBullet()
	{
		var bullet = Resource.Bullet.Instantiate<BaseProjectile>();
		GetTree().Root.AddChild(bullet);
		bullet.Setup(-Player.Transform.Basis.Z);
		bullet.GlobalPosition = Player.GlobalPosition;
		bullet.Rotation = Player.Rotation;
	}
}
