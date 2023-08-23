using Sandbox;
using System.Numerics;

partial class TheStanleyParablePlayer : Player
{
	private TimeSince timeSinceDropped;
	private DamageInfo lastDamage;

	/// <summary>
	/// Default init
	/// </summary>
	public TheStanleyParablePlayer()
	{
	}

	/// <summary>
	/// Initialize using this client
	/// </summary>
	public TheStanleyParablePlayer(IClient cl) : this()
	{
	}

	public override void Respawn()
	{
		SetModel("models/citizen/citizen.vmdl");

		Controller = new WalkController
		{
			WalkSpeed = 60f,
			DefaultSpeed = 180.0f
		};

		if (DevController is NoclipController)
		{
			DevController = null;
		}

		this.ClearWaterLevel();
		EnableAllCollisions = true;
		EnableDrawing = true;
		EnableHideInFirstPerson = true;
		EnableShadowInFirstPerson = false;

		Sandbox.Services.Stats.Increment(Client, "respawn", 1);

		base.Respawn();
	}

	public override void OnKilled()
	{
		base.OnKilled();

		if (lastDamage.HasTag("vehicle"))
		{
			Particles.Create("particles/impact.flesh.bloodpuff-big.vpcf", lastDamage.Position);
			Particles.Create("particles/impact.flesh-big.vpcf", lastDamage.Position);
			PlaySound("kersplat");
		}

		BecomeRagdollOnClient(Velocity, lastDamage.Position, lastDamage.Force, lastDamage.BoneIndex, lastDamage.HasTag("bullet"), lastDamage.HasTag("blast"));

		Controller = null;

		EnableAllCollisions = false;
		EnableDrawing = false;

		foreach (var child in Children)
		{
			child.EnableDrawing = false;
		}
	}

	public override void TakeDamage(DamageInfo info)
	{
		if (info.Attacker.IsValid())
		{
			if (info.Attacker.Tags.Has($"{Client.SteamId}"))
				return;
		}

		if (info.Hitbox.HasTag("head"))
		{
			info.Damage *= 10.0f;
		}

		lastDamage = info;

		base.TakeDamage(info);
	}

	public override PawnController GetActiveController()
	{
		if (DevController != null) return DevController;

		return base.GetActiveController();
	}

	public override void Simulate(IClient cl)
	{
		base.Simulate(cl);

		if (LifeState != LifeState.Alive)
			return;

		var controller = GetActiveController();
		if (controller != null)
		{
			EnableSolidCollisions = !controller.HasTag("noclip");

			SimulateAnimation(controller);
		}

		if ( Input.Pressed( "attack1" ) )
		{
			Input.SetAction("use", true);
		}

		TickPlayerUse();
		SimulateActiveChild(cl, ActiveChild);
	}

	[ConCmd.Admin("noclip")]
	static void DoPlayerNoclip()
	{
		if (ConsoleSystem.Caller.Pawn is TheStanleyParablePlayer basePlayer)
		{
			if (basePlayer.DevController is NoclipController)
			{
				basePlayer.DevController = null;
			}
			else
			{
				basePlayer.DevController = new NoclipController();
			}
		}
	}

	[ConCmd.Admin("kill")]
	static void DoPlayerSuicide()
	{
		if (ConsoleSystem.Caller.Pawn is TheStanleyParablePlayer basePlayer)
		{
			basePlayer.TakeDamage(new DamageInfo { Damage = basePlayer.Health * 99 });
		}
	}

	void SimulateAnimation(PawnController controller)
	{
		if (controller == null)
			return;

		// where should we be rotated to
		var turnSpeed = 0.02f;

		Rotation rotation;

		// If we're a bot, spin us around 180 degrees.
		if (Client.IsBot)
			rotation = ViewAngles.WithYaw(ViewAngles.yaw + 180f).ToRotation();
		else
			rotation = ViewAngles.ToRotation();

		var idealRotation = Rotation.LookAt(rotation.Forward.WithZ(0), Vector3.Up);
		Rotation = Rotation.Slerp(Rotation, idealRotation, controller.WishVelocity.Length * Time.Delta * turnSpeed);
		Rotation = Rotation.Clamp(idealRotation, 45.0f, out var shuffle); // lock facing to within 45 degrees of look direction

		CitizenAnimationHelper animHelper = new CitizenAnimationHelper(this);

		animHelper.WithWishVelocity(controller.WishVelocity);
		animHelper.WithVelocity(controller.Velocity);
		animHelper.WithLookAt(EyePosition + EyeRotation.Forward * 100.0f, 1.0f, 1.0f, 0.5f);
		animHelper.AimAngle = rotation;
		animHelper.FootShuffle = shuffle;
		animHelper.DuckLevel = MathX.Lerp(animHelper.DuckLevel, controller.HasTag("ducked") ? 1 : 0, Time.Delta * 10.0f);
		animHelper.VoiceLevel = Client.Voice.LastHeard < 0.5f ? Client.Voice.CurrentLevel : 0.0f;
		animHelper.IsGrounded = GroundEntity != null;
		animHelper.IsSitting = controller.HasTag("sitting");
		animHelper.IsNoclipping = controller.HasTag("noclip");
		animHelper.IsClimbing = controller.HasTag("climbing");
		animHelper.IsSwimming = this.GetWaterLevel() >= 0.5f;
		animHelper.IsWeaponLowered = false;
		animHelper.MoveStyle = Input.Down("run") ? CitizenAnimationHelper.MoveStyles.Run : CitizenAnimationHelper.MoveStyles.Walk;

		if (ActiveChild is BaseCarriable carry)
		{
			carry.SimulateAnimator(animHelper);
		}
		else
		{
			animHelper.HoldType = CitizenAnimationHelper.HoldTypes.None;
			animHelper.AimBodyWeight = 0.5f;
		}
	}

	public override void StartTouch(Entity other)
	{
		if (timeSinceDropped < 1) return;

		base.StartTouch(other);
	}

	public override float FootstepVolume()
	{
		return Velocity.WithZ(0).Length.LerpInverse(0.0f, 200.0f) * 5.0f;
	}

	public override void FrameSimulate(IClient cl)
	{
		Camera.Rotation = ViewAngles.ToRotation();
		Camera.FieldOfView = Screen.CreateVerticalFieldOfView(Game.Preferences.FieldOfView);

	    if (LifeState != LifeState.Alive && Corpse.IsValid())
		{
			Corpse.EnableDrawing = true;

			var pos = Corpse.GetBoneTransform(0).Position + Vector3.Up * 10;
			var targetPos = pos + Camera.Rotation.Backward * 100;

			var tr = Trace.Ray(pos, targetPos)
				.WithAnyTags("solid")
				.Ignore(this)
				.Radius(8)
				.Run();

			Camera.Position = tr.EndPosition;
			Camera.FirstPersonViewer = null;
		}
		else
		{
			Camera.Position = EyePosition;
			Camera.FirstPersonViewer = this;
		}
	}
}
