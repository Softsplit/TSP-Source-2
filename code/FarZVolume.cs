using Sandbox;

public class FarZVolume : Component
{
	public enum CameraModes
	{
		RenderSkybox,
		DepthOnly
	}

	[Property]
	public float farZ { get; set; }

	[Property]
	public CameraModes CameraMode { get; set; }

	private Vector3 fullExtents;

	private Vector3 position;

	private bool touchingLastFrame;

	private Transform followTransform;

	// private Bounds volumeBounds;

	protected override void OnAwake()
	{
		position = Transform.Position;
		fullExtents = new Vector3( base.Transform.LocalScale.x, base.Transform.LocalScale.y, base.Transform.LocalScale.z );
		// volumeBounds = new Bounds( position, fullExtents );
	}

	protected override void OnStart()
	{
		// followTransform = StanleyController.Instance.Transform;
	}

	protected override void OnFixedUpdate()
	{
		/*
		if ( volumeBounds.Contains( StanleyController.StanleyPosition ) )
		{
			if ( !touchingLastFrame )
			{
				StanleyController.Instance.SetFarZ( farZ, CameraMode );
			}
			touchingLastFrame = true;
		}
		else
		{
			touchingLastFrame = false;
		}
		*/
	}

	public void ToggleDepthOnlySkybox( bool status )
	{
		if ( status )
		{
			CameraMode = CameraModes.DepthOnly;
		}
		else
		{
			CameraMode = CameraModes.RenderSkybox;
		}
	}
}
