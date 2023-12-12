using Sandbox;
namespace TSPS2;

[Title( "Stanley" )]
[Category( "The Stanley Parable" )]
[Icon( "accessible_forward" )]
public partial class Player : Component
{
	[Property] SkinnedModelRenderer Body { get; set; }
	[Property] public CameraComponent Eyes { get; set; }

	[Property] public Vector3 Gravity { get; set; } = new Vector3( 0, 0, 800 );
	[Property] public float Speed { get; set; } = 175f;


	protected override void OnUpdate()
	{
		BuildInput();
		BuildAnimation();
	}

	protected override void OnFixedUpdate()
	{
		SimulateMovement();
	}

	protected override void OnEnabled()
	{
		if ( Body is null )
			return;

		Body.OnFootstepEvent -= OnEvent;
	}

	private void OnEvent( SceneModel.FootstepEvent e )
	{
		Log.Info( "Hi" );
	}
}
