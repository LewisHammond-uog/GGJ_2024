using Godot;

namespace GGJ24.enemy;

public partial class EnemyShooter : Node3D
{
    public bool CanShoot => false;
    public float MaxShootDist => 5f;
}