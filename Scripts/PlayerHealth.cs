using Godot;

namespace GGJ24.Scripts;

public sealed partial class PlayerHealth : Health, IHealable, IArmorRepairable
{
    [Export] private float maxAmour = 100f;
    private float armor;

    public float Armor
    {
        get => armor;
        private set
        {
            armor = Mathf.Clamp(value, 0, maxAmour);
        }
    }
    private float Armor01 => armor / maxAmour;
    
    
    public override void TakeDamage(float damage)
    {
        //
        
        base.TakeDamage(damage);
    }

    public void Heal(float heal)
    {
        CurrentHealth += heal;
    }

    public void RepairArmor(float heal)
    {
        Armor += heal;
    }
}