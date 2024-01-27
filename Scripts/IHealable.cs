namespace GGJ24.Scripts;

public interface IHealable
{
    public void Heal(float heal);
}

public interface IArmorRepairable
{
    public void RepairArmor(float heal);
}