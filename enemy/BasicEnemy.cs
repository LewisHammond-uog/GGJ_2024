using System;
using Godot;
using Godot.Collections;

namespace GGJ24.enemy;

public partial class BasicEnemy : CharacterBody3D
{
	[Export] private NavigationAgent3D agent;
	[Export] private float seeDistance = 1000f;

	[Export] private CharacterBody3D player;

	private bool canSeePlayer = false;
	private State state;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = State.Idle;
		player = GetNode<CharacterBody3D>("/root/TestScene/Player");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		UpdateState();
		
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		canSeePlayer = CanSeePlayer();
	}

	private bool CanSeePlayer()
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(Position, player.Position);
		query.Exclude = new Array<Rid>(new Rid[] { GetRid(), player.GetRid() });
		var result = spaceState.IntersectRay(query);

		return result.Count == 0;
	}

	private void UpdateState()
	{
		switch (state)
		{
			case State.Idle:
				
				break;
			case State.MoveToPlayer:
				break;
			case State.MoveAndShoot:
				break;
			case State.StandAndShoot:
				break;
			case State.Dead:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}
	private enum State
	{
		Idle,
		MoveToPlayer,
		MoveAndShoot,
		StandAndShoot,
		Stunned,
		Dead
	}
}
