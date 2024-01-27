using Godot;
using System;

[GlobalClass]
public partial class WeaponResource : Resource
{
    [Export] public string PrettyName = "";
    [Export] public WeaponTypes WeaponType;
    [Export] public int MaximumAmmo = 0;
    [Export] public int ClipSize = 0;
    [Export] public int AmmoPerShot = 1;
    [Export] public float WindUp = 0f;
    [Export] public float Cooldown = 0f;
    [Export] public Resource WeaponSprite;
    [Export] public Resource AmmoSprite;
    [Export] public PackedScene Bullet;
    [Export] public PackedScene Animation;
}
