using Godot;
using System;
using GGJ24;
using GGJ24.Scripts;

public partial class PlayerController : CharacterBody3D
{
	[Export] public float Speed = 5.0f;
	[Export] public float SprintMod = 2f;
	[Export] public float JumpVelocity = 4.5f;
	[Export] public float SlideSlow = 1f;
	[Export] public float KickForce = 4f;

	[Export] public bool CrouchToggle = true;

	[Export] public float Sensitivity = 1.0f;
	[Export] public Camera3D camera;

	public bool isSliding = false;
	private bool isCrouching = false;
	private bool isKicking = false;
	public Vector2 lookDirection;
	private Vector3 scale = new Vector3(1,0.98f,1);

	// Get the gravity from the project settings to be synced with RigidBody nodes.
	public float gravity = ProjectSettings.GetSetting("physics/3d/default_gravity").AsSingle();

	public override void _Ready()
	{
		Input.MouseMode = Input.MouseModeEnum.Captured;
		Scale = scale;
		CSharpGlobals.pathToPlayer = GetPath();
		this.TryFindNodeOfTypeInChildren<PlayerHealth>().DeathEvent += OnDeath;
	}
	
	
	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion motion)
		{
			lookDirection = motion.Relative * 0.001f;
			if(Input.MouseMode == Input.MouseModeEnum.Captured)
				Look();
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector3 velocity = Velocity;
	
		// Add the gravity.
		if (!IsOnFloor())
			velocity.Y -= gravity * (float)delta;

		// Handle Jump.
		if (Input.IsActionJustPressed("movement_jump") && IsOnFloor())
			velocity.Y = JumpVelocity;

#region Movement
		// Get the input direction and handle the movement/deceleration.
		Vector2 inputDir = Input.GetVector("movement_left", "movement_right", "movement_up", "movement_down");
		Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

		if (CrouchToggle)
		{
			ToggledCrouch();
		}
		else
		{
			isCrouching = Input.IsActionPressed("movement_crouch");
		}
		
		if (!isCrouching)
		{
			isSliding = false;
		}

		bool isSprinting = Input.IsActionPressed("movement_sprint");
		float speedMod = isSprinting && !isCrouching ? Speed * SprintMod : Speed;
		speedMod *= isCrouching ? 0.5f : 1;

		scale = scale.Slerp(new Vector3(1, isCrouching ? 0.5f : 1, 1), 0.4f);
		Scale = scale;

		if (isSprinting && isCrouching && Input.IsActionJustPressed("movement_crouch"))
		{
			isSliding = true;
			speedMod *= 1.2f;
			
			velocity.X = direction.X * speedMod;
			velocity.Z = direction.Z * speedMod;
		}
		
		if (direction != Vector3.Zero && !isSliding && !isKicking)
		{
			velocity.X = direction.X * speedMod;
			velocity.Z = direction.Z * speedMod;
		}
		else if (isSliding)
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, (float)(SlideSlow * delta));
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, (float)(SlideSlow * delta));
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
			velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
		}

		if (velocity.Length() < 0.2f)
		{
			isSliding = false;
			isKicking = false;
		}

		Velocity = velocity;
		MoveAndSlide();
#endregion
	}

	private void ToggledCrouch()
	{
		if (Input.IsActionJustPressed("movement_crouch"))
		{
			isCrouching = !isCrouching;
		}
	}

	private void Look()
	{
		Vector3 lookVector = camera.Rotation;
		lookVector.X = Math.Clamp(lookVector.X - (lookDirection.Y * Sensitivity), -1.5f,1.5f);
		camera.Rotation = lookVector;

		Vector3 playerRotation = Rotation;
		playerRotation.Y -= lookDirection.X * Sensitivity;
		Rotation = playerRotation;
	}
	
	private void OnDeath(object sender, EventArgs e)
	{
		SetProcess(false);
		SetPhysicsProcess(false);
		SetProcessInput(false);
		SetProcessUnhandledInput(false);
		this.TryFindNodeOfTypeInChildren<PlayerHealth>().DeathEvent -= OnDeath;
	}

	public void OnKick()
	{
		isKicking = true;
		Vector3 direction = (Transform.Basis * Vector3.Forward).Normalized();
		Velocity = direction * KickForce * 10;
		MoveAndSlide();
	}
}






