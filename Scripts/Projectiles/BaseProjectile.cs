using Godot;
using System;
using GGJ24;
using GGJ24.enemy;
using GGJ24.Scripts;

public partial class BaseProjectile : RigidBody3D
{
    [Export] protected float speed = 5f;
    [Export] protected float knockBackForce = 2f;
    [Export] protected float damage = 3f;
    [Export] protected float lifeTime = 10f;
    
    public Timer LifeTimer;
    
    public void Setup(Vector3 direction)
    {
        LinearVelocity = direction.Normalized() * speed;
        BodyEntered += OnBodyEntered;
        
        LifeTimer = new Timer();
        LifeTimer.Autostart = false;
        LifeTimer.OneShot = true;
        LifeTimer.Stop();
        AddChild(LifeTimer);
        LifeTimer.Timeout += Destroy;
        
        LifeTimer.Start(lifeTime);
    }
    
    public void OnBodyEntered(Node body)
    {
        try
        {
            var damagable = body.TryFindNodeOfTypeInChildren<IDamageable>();
            if (body is BasicEnemy enemy)
            {
                Vector3 knockbackDirection = LinearVelocity.Normalized();
                knockbackDirection.Y = 0.2f;
                enemy.StartKnockback(knockbackDirection * knockBackForce);
            }

            body.Call("explode");
            damagable?.TakeDamage(damage);
            Destroy();
        }
        catch
        {
            // ignored
        }
    }

    private void Destroy()
    {
        BodyEntered -= OnBodyEntered;
        QueueFree();
    }
}
