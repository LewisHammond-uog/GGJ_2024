using Godot;

namespace GGJ24;

public partial class Bullet : RigidBody3D
{
	[Export] private float speed = 5f;
	[Export] private float damage = 3f;
	
	public void Setup(Vector3 direction)
	{
		LinearVelocity = direction.Normalized() * speed;
	}
	
	private void OnBodyEntered(Node body)
	{
		var damagable = body.TryFindNodeOfTypeInChildren<IDamageable>();
		damagable?.TakeDamage(damage);
		QueueFree();
	}
}

