using Sandbox;
using System.Linq;
using System.Numerics;
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

		if ( Game.Server.MapIdent == "softsplit.tsp_seriousroom" )
        {
            ConsoleSystem.Run( "tsp_seriousroom" );
        }

        if (Game.Server.MapIdent == "softsplit.tsp_map")
        {
            ConsoleSystem.Run("tsp_map");
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
            Sound.FromEntity("seriousroom_1", (Entity)ConsoleSystem.Caller.Client.Pawn);
        }
        else if ( joinCount == 1 )
        {
            Sound.FromEntity("seriousroom_2", (Entity)ConsoleSystem.Caller.Client.Pawn);
        }
        else if ( joinCount == 2 )
        {
            Sound.FromEntity("seriousroom_3", (Entity)ConsoleSystem.Caller.Client.Pawn);
        }
        else if ( joinCount >= 3 )
        {
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

    [ConCmd.Server("tsp_map")]
    public static void MapCmd()
    {
        
    }
}
