using System;
using Godot;
using Godot.Collections;

namespace GGJ24.enemy;

public partial class BasicEnemy : CharacterBody3D
{
	[Export] private NavigationAgent3D agent;
	[Export] private EnemyShooter shootComp;
	[Export] private float seeDistance = 1000f;
	[Export] private float idealDistanceToPlayer = 1f;
	[Export] private float standStillDistance = 0.2f;
	[Export] private float moveSpeed = 2f;

	[Export] private CharacterBody3D player;

	private bool canSeePlayer = false;
	private Vector3? playerLastPos;
	private Vector3 movePos;
	private State state;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = State.Idle;
		player = GetNode<CharacterBody3D>("/root/TestScene/Player");
		movePos = Position;
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
		calculateMove(delta);
	}

	private void calculateMove(double dt)
	{
		agent.TargetPosition = movePos;
		Vector3 nextPos = agent.GetNextPathPosition();
		Vector3 vel = Position.DirectionTo(nextPos) * moveSpeed;
		agent.Velocity = vel;
	}

	private bool CanSeePlayer()
	{
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(Position, player.Position);
		query.Exclude = new Array<Rid>(new Rid[] { GetRid(), player.GetRid() });
		var result = spaceState.IntersectRay(query);

		if (result.Count == 0)
		{
			playerLastPos = player.Position;
		}
		
		return result.Count == 0;
	}

	//THIS IS A MESS
	private void UpdateState()
	{
		switch (state)
		{
			case State.Idle:
				if (canSeePlayer)
				{
					state = State.MoveToPlayer;
					break;
				}
				if (playerLastPos != null)
				{
					movePos = playerLastPos.Value;
				}
				break;
			case State.MoveToPlayer:
			{
				//If we are close enough to shoot and can shoot the player
				if (shootComp is { CanShoot: true } && Position.DistanceTo(player.Position) < shootComp.MaxShootDist)
				{
					state = State.MoveAndShoot;
				}

				Vector2 intercept = GetPlayerRadiusInterceptXZ();
				movePos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
			}
				break;
			case State.MoveAndShoot:
			{
				if (shootComp is { CanShoot: true } && Position.DistanceTo(player.Position) > shootComp.MaxShootDist)
				{
					state = State.MoveToPlayer;
					break;
				}

				Vector2 intercept = GetPlayerRadiusInterceptXZ();
				Vector3 interceptPos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
				if (Position.DistanceTo(interceptPos) < standStillDistance)
				{
					state = State.StandAndShoot;
					break;
				}

				movePos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
				break;
			}
			case State.StandAndShoot:
			{
				Vector2 intercept = GetPlayerRadiusInterceptXZ();
				Vector3 interceptPos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
				if (Position.DistanceTo(interceptPos) > standStillDistance)
				{
					state = State.MoveAndShoot;
				}

				break;
			}
			case State.Dead:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private Vector2 GetPlayerRadiusInterceptXZ()
	{
		Vector2 playerCenter = new Vector2(player.Position.X, player.Position.Z);
		Vector2 myCenter = new Vector2(Position.X, Position.Y);
		Vector2 intercept = playerCenter + (myCenter - playerCenter).Normalized() * idealDistanceToPlayer;
		return intercept;
	}
	
	private void AgentVelocityComputed(Vector3 safe_velocity)
	{
		Velocity = Velocity.MoveToward(safe_velocity, 0.25f);
		MoveAndSlide();
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



