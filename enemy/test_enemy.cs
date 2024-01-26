using Godot;
using System;

public partial class test_enemy : CharacterBody3D
{
	[Export] private NavigationAgent3D agent;
	[Export] public string a;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print(agent.MaxSpeed);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
