using Godot;
using System;
using GGJ24;

public partial class BaseProjectile : RigidBody3D
{
    [Export] protected float speed = 5f;
    [Export] protected float damage = 3f;
    [Export] protected float lifeTime = 10f;
    
    public Timer LifeTimer;
    
    
    
    public void Setup(Vector3 direction)
    {
        LinearVelocity = direction.Normalized() * speed;
        
        LifeTimer = new Timer();
        LifeTimer.Autostart = false;
        LifeTimer.OneShot = true;
        LifeTimer.Stop();
        AddChild(LifeTimer);
        LifeTimer.Timeout += Destroy;
        
        LifeTimer.Start(lifeTime);
    }
	
    private void OnBodyEntered(Node body)
    {
        var damagable = body.TryFindNodeOfTypeInChildren<IDamageable>();
        damagable?.TakeDamage(damage);
        Destroy();
    }

    private void Destroy()
    {
        QueueFree();
    }
}
