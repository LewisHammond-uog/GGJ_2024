using Godot;

namespace GGJ24.enemy;

public partial class EnemyShooter : Node3D
{
	public bool CanShoot => shootTimer.IsStopped() || shootTimer.TimeLeft <= double.Epsilon;
	[Export] public float MaxShootDist;

	[Export] private double shootInterval = 5f;
	[Export] private PackedScene bulletPrefab;

	private Timer shootTimer;

	public override void _Ready()
	{
		base._Ready();
		shootTimer = new Timer();
		shootTimer.Autostart = false;
		shootTimer.OneShot = true;
		shootTimer.Stop();
		AddChild(shootTimer);
	}

	public void TryShoot()
	{
		if (!CanShoot)
		{
			return;
		}
		
		var spawnedBullet = bulletPrefab.Instantiate<Bullet>();
		GetTree().Root.AddChild(spawnedBullet);
		spawnedBullet.GlobalPosition = GlobalPosition;
		spawnedBullet.Setup(-GlobalTransform.Basis.Z);
		shootTimer.Start(shootInterval);
	}
}
