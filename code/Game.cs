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

		if ( Game.Server.MapIdent == "softsplit.seriousroom" )
        {
            ConsoleSystem.Run( "tsp_seriousroom" );
        }

        if (Game.Server.MapIdent == "map")
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
        SetRandomMugColors(); 

        
    }

    private static void SetRandomMugColors()
    {
        System.Random random = new System.Random();

        string[] coffeecup1Colors = new string[]
        {
            "208 224 152 1",
            "186 141 235 1",
            "147 230 180 1",
            "232 145 157 1"
        };

        string[] coffeecup2Colors = new string[]
        {
            "147 188 230 1",
            "228 197 148 1",
            "232 145 157 1",
            "232 145 157 1"
        };

        string[] coffeecup3Colors = new string[]
        {
            "232 145 157 1",
            "140 152 236 1",
            "236 224 140 1"
        };

        // Randomly select colors for each mug
        string randomColor1 = coffeecup1Colors[random.Next(coffeecup1Colors.Length)];
        string randomColor2 = coffeecup2Colors[random.Next(coffeecup2Colors.Length)];
        string randomColor3 = coffeecup3Colors[random.Next(coffeecup3Colors.Length)];

        Entity coffeeCup1 = FindByName("coffeecup1");
        Entity coffeeCup2 = FindByName("coffeecup2");
        Entity coffeeCup3 = FindByName("coffeecup3");

        // Set the selected colors for the mugs using FireInput
        coffeeCup1.FireInput("SetColor", (Entity)ConsoleSystem.Caller.Client.Pawn, randomColor1);
        coffeeCup2.FireInput("SetColor", (Entity)ConsoleSystem.Caller.Client.Pawn, randomColor2);
        coffeeCup3.FireInput("SetColor", (Entity)ConsoleSystem.Caller.Client.Pawn, randomColor3);
    }
}
