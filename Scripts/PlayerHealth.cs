using Godot;

namespace GGJ24.Scripts;

public sealed partial class PlayerHealth : Health, IHealable, IArmorRepairable
{
    [Export] private PackedScene mainMenu;
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

    public override void _Ready()
    {
        base._Ready();
        Armor = maxAmour;
    }

    public override void TakeDamage(float damage)
    {
        var armorRemval = Mathf.Min(damage * 0.33f, Armor);
        damage -= armorRemval;
        Armor -= armorRemval;
        base.TakeDamage(damage);

        if (CurrentHealth == 0)
        {
            Input.MouseMode = Input.MouseModeEnum.Visible;
            GetTree().ChangeSceneToPacked(mainMenu);
        }
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