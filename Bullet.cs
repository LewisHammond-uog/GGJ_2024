using Godot;

namespace GGJ24;

public partial class Bullet : RigidBody3D
{
    [Export] private float speed = 5f;
    
    public void Setup(Vector3 direction)
    {
        LinearVelocity = direction.Normalized() * speed;
    }
}