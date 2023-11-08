using Sandbox;
using System.Linq;
using System.Threading.Tasks;

partial class TSPGame : GameManager
{
	public TSPGame()
	{
	}

	public override void ClientJoined( IClient cl )
	{
		base.ClientJoined( cl );
		var player = new Stanley( cl );

		cl.Pawn = player;

		player.Respawn();
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}
}