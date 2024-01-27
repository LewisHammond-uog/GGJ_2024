using Godot;

namespace GGJ24.Scripts;

public partial class Health : Node, IDamageable
{
	[Signal] public delegate void OnDeathEventHandler();
	
	[Export] private float maxHealth = 100f;
	private float _currentHealth;
	public float CurrentHealth
	{
		get => _currentHealth;
		protected set
		{
			_currentHealth = Mathf.Clamp(value, 0, maxHealth);
			if(_currentHealth <= 0)
			{
				EmitSignal(nameof(OnDeathEventHandler));
				GD.Print("DEAD");
			}
		}
	}
	
	public override void _Ready()
	{
		base._Ready();
		_currentHealth = maxHealth;
	}

	public void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
	}
}
