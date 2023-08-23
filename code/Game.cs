using Sandbox;
using System.Linq;
using System.Threading.Tasks;

partial class TheStanleyParable : GameManager
{
	public TheStanleyParable()
	{
		if ( Game.IsServer )
		{
			// Create the HUD
			_ = new TheStanleyParableHud();
		}
	}

	public override void ClientJoined( IClient cl )
	{
		base.ClientJoined( cl );
		var player = new TheStanleyParablePlayer( cl );

		cl.Pawn = player;

		player.Respawn();

		if ( Game.Server.MapIdent == "softsplit.seriousroom" )
        {
            ConsoleSystem.Run( "tsp_seriousroom" );
        }
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	public class PlayerData
    {
        public int JoinCount { get; set; }
    }

    [ConCmd.Server( "tsp_seriousroom" )]
    public static void SeriousRoomCmd()
    {
        int joinCount = GetJoinCount();

        if ( joinCount == 0 )
        {
            FireMapEntity( Game.LocalPawn, "seriouspass1" );
        }
        else if ( joinCount == 1 )
        {
            FireMapEntity( Game.LocalPawn, "seriouspass2" );
        }
        else if ( joinCount == 2 )
        {
            FireMapEntity( Game.LocalPawn, "seriouspass3" );
        }
        else if ( joinCount >= 3 )
        {
            FireMapEntity( Game.LocalPawn, "seriouspass4" );
        }

        IncrementJoinCount();
    }

    private static int GetJoinCount()
    {
        string path = $"player_data_{Game.SteamId}.json";
        if ( FileSystem.Data.FileExists( path ) )
        {
            PlayerData data = FileSystem.Data.ReadJson<PlayerData>( path );
            return data.JoinCount;
        }

        return 0;
    }

    private static void IncrementJoinCount()
    {
        string path = $"player_data_{Game.SteamId}.json";
        int joinCount = GetJoinCount();
        joinCount++;
        PlayerData data = new() { JoinCount = joinCount };
        FileSystem.Data.WriteJson( path, data );
    }

    private static void FireMapEntity( IEntity activator, string entityName )
    {
        Entity entity = Entity.FindByName( entityName );

        entity?.FireInput( "Trigger", activator );
    }
}
