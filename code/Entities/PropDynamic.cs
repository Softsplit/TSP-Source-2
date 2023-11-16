using Editor;
using Sandbox.ModelEditor.Nodes;
using System;

namespace Sandbox;

/// <summary>
/// A static prop that can play animations. If a door is wanted, please use the door entity.
/// </summary>
[Library( "prop_dynamic" ), HammerEntity]
[Model( Archetypes = ModelArchetype.animated_model ), RenderFields, VisGroup( VisGroup.Dynamic ), Tag( "PropDynamic" )]
[Title( "Animated Entity" ), Category( "Gameplay" ), Icon( "animation" )]
public partial class AnimatedMapEntity : AnimatedEntity
{
	/// <summary>
	/// The name of the idle animation that this prop will revert to whenever it finishes a random or forced animation.
	/// </summary>
	[Property, Net, FGDType( "sequence" )]
	string DefaultAnimation { set; get; }

	/// <summary>
	/// Allow this entity to use its animgraph
	/// </summary>
	[Property, Net]
	bool UseAnimationGraph { set; get; } = false;

	/// <summary>
	/// If set, the prop will not loop its animation, but hold the last frame.
	/// </summary>
	[Property, Net]
	bool HoldAnimation { set; get; } = false;

	/// <summary>
	/// Enable this to animate on the server, such as models with physics bodies attached to bones. Use sparingly as there is a performance cost.
	/// </summary>
	[Property, Title( "Animate On Server" )]
	bool DoAnimateOnServer { set; get; } = false;

	/// <summary>
	/// Whether the animated prop should have collisions.
	/// </summary>
	[Property]
	bool Collisions { set; get; } = true;

	/// <summary>
	/// If the model supports break pieces and has prop_data with health, this option can be used to allow the door to break like a normal prop would.
	/// </summary>
	[Property]
	public bool Breakable { get; set; } = false;

	/// <summary>
	/// Initial animation playback rate.
	/// </summary>
	[Property, MinMax( 0, 5 )]
	float AnimationSpeed{ get; set; } = 1.0f;

	public override void Spawn()
	{
		base.Spawn();

		SetupPhysicsFromModel( PhysicsMotionType.Keyframed );
		EnableAllCollisions = Collisions;

		AnimateOnServer = DoAnimateOnServer;
		PlaybackRate = AnimationSpeed;

		Setup();
	}

	public override void OnClientActive( IClient cl )
	{
		Setup();
	}

	void Setup()
	{
		// Destroy the anim graph if needed
		UseAnimGraph = UseAnimationGraph;

		SetAnimation( DefaultAnimation );
	}

	#region Breakable

	public override void OnNewModel( Model model )
	{
		base.OnNewModel( model );

		// When a model is reloaded, all entities get set to NULL model first
		if ( model == null || model.IsError ) return;

		if ( !Game.IsServer ) return;

		if ( model.TryGetData( out ModelPropData propInfo ) )
		{
			Health = propInfo.Health;
		}

		// If health is unset, set it to -1 - which means it cannot be destroyed
		if ( Health <= 0 ) Health = -1;
	}

	DamageInfo LastDamage;

	/// <summary>
	/// Fired when the entity gets damaged, even if it is unbreakable.
	/// </summary>
	protected Output OnDamaged { get; set; }

	public override void TakeDamage( DamageInfo info )
	{
		// The door was damaged, even if its unbreakable, we still want to fire it
		// TODO: Add damage type as argument? Or should it be the new health value?
		OnDamaged.Fire( this );

		if ( !Breakable ) return;

		base.TakeDamage( info );

		LastDamage = info;
	}

	/// <summary>
	/// Fired when the entity gets destroyed.
	/// </summary>
	protected Output OnBreak { get; set; }

	public override void OnKilled()
	{
		if ( LifeState != LifeState.Alive )
			return;

		var result = new Breakables.Result();
		result.CopyParamsFrom( LastDamage );
		Breakables.Break( this, result );

		OnBreak.Fire( LastDamage.Attacker );

		base.OnKilled();
	}

	/// <summary>
	/// Causes this prop to break, regardless if it is actually breakable or not. (i.e. ignores health and whether the model has gibs)
	/// </summary>
	[Input]
	public void Break()
	{
		OnKilled();
		LifeState = LifeState.Dead;
		Delete();
	}

	#endregion

	/// <summary>
	/// Fired whenever a new animation has begun playing.
	/// </summary>
	Output OnAnimationBegun { get; set; }

	/// <summary>
	/// Fired whenever an animation is complete.
	/// </summary>
	Output OnAnimationDone { get; set; }

	void PlayAnimation( string name )
	{
		if ( UseAnimGraph )
		{
			// TODO: Find a way to cancel this when it is complete
			//DirectPlayback.Play( name );
			return;
		}
		else
		{
			CurrentSequence.Name = name;
		}

		if ( Game.IsServer )
		{
			OnAnimationBegun.Fire( this );
			SetAnimationRPC( name );
		}
	}

	[ClientRpc]
	void SetAnimationRPC( string name )
	{
		PlayAnimation( name );
	}

	protected override void OnSequenceFinished( bool looped )
	{
		//DirectPlayback.Cancel();

		if ( !HoldAnimation )
		{
			SetAnimation( DefaultAnimation );
		}

		if ( Game.IsServer ) OnAnimationDone.Fire( this );
	}

	#region Inputs

	/// <summary>
	/// Play a specific animation (sequence) on the entity.
	/// The parameter should be the name of the animation.
	/// </summary>
	[Input]
	public void SetAnimation( string name )
	{
		PlayAnimation( name );
	}

	/// <summary>
	/// Sets the default animation. This is the animation will auto repeat if looping is enabled.
	/// </summary>
	[Input]
	void SetDefaultAnimation( string name )
	{
		DefaultAnimation = name;
	}

	/// <summary>
	/// Sets the animation playback rate. 1 is normal, 0.5 is half speed, etc.
	/// </summary>
	[Input]
	void SetPlaybackRate( float rate )
	{
		PlaybackRate = rate;
	}

	/// <summary>
	/// Set an animation graph parameter. Format: "name=value"
	/// </summary>
	[Input]
	void SetAnimGraphParameterBool( string data )
	{
		string[] args = data.Split( new [] { '=' } );
		if ( args.Length != 2 )
		{
			Log.Warning( $"AnimatedMapEntity.SetAnimGraphParameterBool was given invalid input \"{args}\", expceted \"name=value\"!" );
			return;
		}

		SetAnimParameter( args[ 0 ], bool.Parse( args[ 1 ] ) );
	}

	/// <summary>
	/// Set an animation graph parameter. Format: "name=value"
	/// </summary>
	[Input]
	void SetAnimGraphParameterInt( string data )
	{
		string[] args = data.Split( new [] { '=' } );
		if ( args.Length != 2 )
		{
			Log.Warning( $"AnimatedMapEntity.SetAnimGraphParameterInt was given invalid input \"{args}\", expceted \"name=value\"!" );
			return;
		}

		SetAnimParameter( args[ 0 ], int.Parse( args[ 1 ] ) );
	}

	/// <summary>
	/// Set an animation graph parameter. Format: "name=value"
	/// </summary>
	[Input]
	void SetAnimGraphParameterFloat( string data )
	{
		string[] args = data.Split( new [] { '=' } );
		if ( args.Length != 2 )
		{
			Log.Warning( $"AnimatedMapEntity.SetAnimGraphParameterFloat was given invalid input \"{args}\", expceted \"name=value\"!" );
			return;
		}

		SetAnimParameter( args[ 0 ], float.Parse( args[ 1 ] ) );
	}

	/// <summary>
	/// Set an animation graph parameter. Format: "name=value"
	/// </summary>
	[Input]
	void SetAnimGraphParameterVector( string data )
	{
		string[] args = data.Split( new [] { '=' } );
		if ( args.Length != 2 )
		{
			Log.Warning( $"AnimatedMapEntity.SetAnimGraphParameterVector was given invalid input \"{args}\", expceted \"name=value\"!" );
			return;
		}

		SetAnimParameter( args[ 0 ], Vector3.Parse( args[ 1 ] ) );
	}

	/// <summary>
	/// Set an animation graph parameter. Format: "name=value"
	/// </summary>
	[Input]
	void SetAnimGraphParameterAngles( string data )
	{
		string[] args = data.Split( new [] { '=' } );
		if ( args.Length != 2 )
		{
			Log.Warning( $"AnimatedMapEntity.SetAnimGraphParameterVector was given invalid input \"{args}\", expceted \"name=value\"!" );
			return;
		}

		SetAnimParameter( args[ 0 ], Angles.Parse( args[ 1 ] ).ToRotation() );
	}

	#endregion

	#region Prop Inputs

	/// <summary>
	/// Deletes this prop.
	/// </summary>
	[Input( "Delete" )]
	protected void DeleteInput()
	{
		Delete();
	}

	/// <summary>
	/// Sets the scale of the prop, affecting physics and visual scale.
	/// </summary>
	[Input]
	protected void SetScale( float scale )
	{
		Scale = scale.Clamp( 0.1f, 100f );

		if ( PhysicsGroup != null )
		{
			// Set mass as well. Without this objects feel really floaty at large scales.
			PhysicsGroup.RebuildMass(); // First rebuild mass so we are at the default.
			PhysicsGroup.Mass *= scale.Clamp( 1f, 100f );

			// Wake up physics
			PhysicsGroup.Sleeping = false;
		}
	}

	/// <summary>
	/// Sets the material group of the props' model by name, as set in ModelDoc.
	/// </summary>
	[Input( "SetMaterialGroup" )]
	protected void SetMaterialGroupInput( string group )
	{
		SetMaterialGroup( group );
	}

	/// <summary>
	/// Sets the body group of the props' model by name, as set in ModelDoc.
	/// Format is "name,option"
	/// </summary>
	[Input( "SetBodyGroup" )]
	protected void SetBodyGroupInput( string group )
	{
		string[] opts = group.Split( new [] { ' ', ',' } );
		if ( opts.Length != 2 )
		{
			Log.Warning( $"Prop.SetBodyGroup was given invalid input \"{group}\", expceted \"name,option\"!" );
			return;
		}

		SetBodyGroup( opts[ 0 ], opts[ 1 ].ToInt() );
	}

	/// <summary>
	/// Enables or disables collisions on this prop.
	/// </summary>
	[Input]
	protected void SetCollisions( bool enabled )
	{
		EnableAllCollisions = enabled;
	}

	/// <summary>
	/// Enables or disables collisions on this prop.
	/// </summary>
	[Input]
	protected void SetVisible( bool enabled )
	{
		EnableDrawing = enabled;
	}

	#endregion
}