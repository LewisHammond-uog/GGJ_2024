using System;
using GGJ24.Scripts;
using Godot;
using Godot.Collections;

namespace GGJ24.enemy;

public partial class BasicEnemy : CharacterBody3D
{
	[Export] private NavigationAgent3D agent;
	[Export] private EnemyShooter shootComp;
	[Export] private float seeDistance = 1000f;
	[Export] private float idealDistanceToPlayer;
	[Export] private float standStillDistance = 0.2f;
	[Export] private float moveSpeed = 2f;
	[Export] private AnimatedSprite3D anim;
	private CharacterBody3D player => GetNode<CharacterBody3D>(CSharpGlobals.pathToPlayer);

	private bool canSeePlayer = false;
	private Vector3? playerLastPos;
	private Vector3 movePos;
	private State state;

	//See https://github.com/godotengine/godot/issues/76349 - issue where gridmaps cannot be raycasted on frame 1
	private bool isFirstFrame = true;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		state = State.Idle;
		movePos = Position;
		anim.Animation = "Idle";
		this.TryFindNodeOfTypeInChildren<Health>().DeathEvent += DeathEvent;
	}

	public void StartKnockback(Vector3 knockbackForce)
	{
		state = State.Stunned;
		Velocity = knockbackForce;
		anim.Animation = "GetHit";
		anim.Play();
		anim.AnimationFinished += RevertToIdle;
		MoveAndSlide();
	}

	private void RevertToIdle()
	{
		anim.Animation = "Idle";
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		//If dead then destroy, in future wait for an animation to finish
		if (state == State.Dead)
		{
			anim.Animation = "TakeDamage";
			anim.Play();
			anim.AnimationFinished += Destroy;
		}
	}

	private void Destroy()
	{
		QueueFree();
	}

	public override void _PhysicsProcess(double delta)
	{
		base._PhysicsProcess(delta);
		UpdateState();

		if (!isFirstFrame)
		{
			canSeePlayer = CanSeePlayer();
		}
		isFirstFrame = false;

		if (state is State.Dead or State.Stunned)
		{
			return;
		}
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
		if (player == null)
		{
			return false;
		}
		
		var spaceState = GetWorld3D().DirectSpaceState;
		var query = PhysicsRayQueryParameters3D.Create(Position + new Vector3(0,1,0), player.Position + new Vector3(0,1,0));
		query.CollideWithAreas = false;
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
				if (!canSeePlayer)
				{
					state = State.Idle;
				} else if (shootComp is { CanShoot: true } && Position.DistanceTo(player.Position) < shootComp.MaxShootDist)
				{
					state = State.MoveAndShoot;
				}

				Vector2 intercept = GetPlayerRadiusInterceptXZ();
				movePos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
			}
				break;
			case State.MoveAndShoot:
			{
				if (!canSeePlayer)
				{
					state = State.Idle;
				} else if (shootComp is { CanShoot: false } || Position.DistanceTo(player.Position) > shootComp.MaxShootDist)
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

				shootComp.TryShoot();
				movePos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
				break;
			}
			case State.StandAndShoot:
			{
				if (!canSeePlayer)
				{
					state = State.Idle;
				}
				
				Vector2 intercept = GetPlayerRadiusInterceptXZ();
				Vector3 interceptPos = new Vector3(intercept.X, player.Position.Y, intercept.Y);
				if (Position.DistanceTo(interceptPos) > standStillDistance)
				{
					state = State.MoveAndShoot;
				}
				shootComp.TryShoot();
				break;
			}
			case State.Stunned:
				ApplyKnockback();
				break;
			case State.Dead:
				break;
			default:
				throw new ArgumentOutOfRangeException();
		}
	}

	private Vector2 GetPlayerRadiusInterceptXZ()
	{
		Vector2 playerCenter = new Vector2(player.Position.X, player.Position.Z);
		Vector2 myCenter = new Vector2(Position.X, Position.Z);
		Vector2 intercept = playerCenter + (myCenter - playerCenter).Normalized() * idealDistanceToPlayer;
		return intercept;
	}
	
	private void AgentVelocityComputed(Vector3 safe_velocity)
	{
		safe_velocity.Y = player.Position.Y - Position.Y;
		Velocity = Velocity.MoveToward(safe_velocity, 0.25f);
		Vector3 lookAt = player.Position;
		lookAt.Y = Position.Y;
		LookAt(lookAt);
		MoveAndSlide();
	}

	private void ApplyKnockback()
	{
		Velocity.MoveToward(Vector3.Zero, 0.2f);
		Vector3 lookAt = player.Position;
		lookAt.Y = Position.Y;
		LookAt(lookAt);
		MoveAndSlide();

		if (Velocity.Length() < 0.2f)
		{
			state = State.MoveToPlayer;
		}
	}

	private void DeathEvent(object sender, EventArgs e)
	{
		state = State.Dead;
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
