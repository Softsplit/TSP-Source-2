using Sandbox;
namespace TSPS2;

public partial class Player
{
	public void BuildAnimation()
	{
		var cc = GameObject.Components.Get<CharacterController>();
		if ( cc is null ) return;

		float rotateDifference = 0;

		// rotate body to look angles
		if ( Body is not null )
		{
			var targetAngle = new Angles( 0, EyeAngles.yaw, 0 ).ToRotation();

			var v = cc.Velocity.WithZ( 0 );

			if ( v.Length > 10.0f )
			{
				targetAngle = Rotation.LookAt( v, Vector3.Up );
			}

			rotateDifference = Body.Transform.Rotation.Distance( targetAngle );

			if ( rotateDifference > 50.0f || cc.Velocity.Length > 10.0f )
			{
				Body.Transform.Rotation = Rotation.Lerp( Body.Transform.Rotation, targetAngle, Time.Delta * 2.0f );
			}
		}

		var AnimationHelper = GameObject.Components.Get<CitizenAnimation>();
		if ( AnimationHelper is not null )
		{
			AnimationHelper.WithVelocity( cc.Velocity );
			AnimationHelper.WithWishVelocity( WishDirection * Speed );
			AnimationHelper.IsGrounded = cc.IsOnGround;
			AnimationHelper.FootShuffle = rotateDifference;
			AnimationHelper.WithLook( EyeAngles.Forward, 1, 1, 1.0f );
			AnimationHelper.MoveStyle = CitizenAnimation.MoveStyles.Auto;
		}
	}
}
