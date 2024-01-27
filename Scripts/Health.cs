using System;
using Godot;

namespace GGJ24.Scripts;

public partial class Health : Node, IDamageable
{
	public EventHandler DeathEvent;
	
	[Export] private float maxHealth = 100f;
	private bool processedDeath = false;
	private float _currentHealth;
	
	public float CurrentHealth
	{
		get => _currentHealth;
		protected set
		{
			_currentHealth = Mathf.Clamp(value, 0, maxHealth);
			if(_currentHealth <= 0)
			{
				DeathEvent?.Invoke(this, null!);
				processedDeath = true;
			}
		}
	}
	public float Health01 => _currentHealth / maxHealth;
	
	public override void _Ready()
	{
		base._Ready();
		_currentHealth = maxHealth;
	}

	public virtual void TakeDamage(float damage)
	{
		CurrentHealth -= damage;
	}
}
