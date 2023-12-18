public class PlayerStart : HammerEntity
{
	public bool isMaster;

	protected override void OnStart()
	{
		Respawn();
	}

	public void Respawn()
	{
		StanleyController.TeleportType style = StanleyController.TeleportType.PlayerStartMaster;
		if ( !isMaster )
		{
			style = StanleyController.TeleportType.PlayerStart;
		}
		// StanleyController.Instance.Teleport( style, base.Transform.Position, -base.Transform.Up, useAngle: true, freezeAtStartOfTeleport: false, unfreezeAtEndOfTeleport: true, Transform );
	}

	protected override void DrawGizmos()
	{
		/*
		Gizmos.matrix = base.Transform.LocalToWorldMatrix;
		float num = 0.64f;
		for ( int i = 0; i < 10; i++ )
		{
			Gizmos.color = Color.Blue;
			Gizmos.DrawRay( Vector3.Forward * MathX.Lerp( num * 0.75f, num, (float)i / 9f ), Vector3.Down * 0.25f );
		}
		Gizmos.color = Color.Green;
		Gizmos.DrawCube( Vector3.Zero + Vector3.Forward * (num / 2f), new Vector3( 0.05f, 0.05f, num ) );
		*/
	}
}
