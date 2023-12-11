using Sandbox;
namespace Parable;

public partial class Player
{
	/// <summary>
	/// Direction player is looking.
	/// </summary>
	public Angles EyeAngles;
	
	/// <summary>
	/// Direction player wants to move.
	/// </summary>
	public Vector3 WishDirection;
	
	/// <summary>
	/// Build player input.
	/// </summary>
	public void BuildInput()
	{
		EyeAngles.pitch = ( EyeAngles.pitch + Input.MouseDelta.y * 0.1f ).Clamp( -90, 90 );
		EyeAngles.yaw -= Input.MouseDelta.x * 0.1f;

		// Camera
		Eyes.Transform.Rotation = EyeAngles.ToRotation();
		// Movement
		WishDirection = ( Input.AnalogMove * EyeAngles.WithPitch( 0 ).ToRotation() ).Normal;

	}
	
	/// <summary>
	/// Simulate player movement. Copied from scene-staging.
	/// </summary>
	public void SimulateMovement()
	{
		var WishVelocity = WishDirection * Speed;
		var cc = GameObject.Components.Get<CharacterController>();

		if ( cc.IsOnGround )
		{
			cc.Velocity = cc.Velocity.WithZ( 0 );
			cc.Accelerate( WishVelocity );
			cc.ApplyFriction( 4.0f );
		}
		else
		{
			cc.Velocity -= Gravity * Time.Delta * 0.5f;
			cc.Accelerate( WishVelocity.ClampLength( 50 ) );
			cc.ApplyFriction( 0.1f );
		}
		
		cc.Move();

		if ( !cc.IsOnGround ) cc.Velocity -= Gravity * Time.Delta * 0.5f;
		else cc.Velocity = cc.Velocity.WithZ( 0 );
	}
}
