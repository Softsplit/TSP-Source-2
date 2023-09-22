using Editor;
using Sandbox;

/// <summary>
/// Fires outputs when a map spawns. If 'Remove on fire' flag is set the logic_auto is deleted after firing. It can be set to check a global state before firing. This allows you to only fire events based on what took place in a previous map.
/// </summary>
[Library("logic_auto")]
[HammerEntity]
[VisGroup(VisGroup.Logic)]
[EditorSprite("editor/logic_relay.vmat")]
[Title("Auto"), Category("Logic"), Icon("autorenew")]
public partial class LogicAuto : Entity
{
    public enum GlobalStateType
    {
        None = 0,
        GordonPreCriminal = 1,
        AntlionsArePlayerAllies = 2,
        SuitSprintFunctionNotYetEnabled = 3,
        SuperPhysGunIsEnabled = 4,
        FriendlyEncounterSequence = 5,
        GordonIsInvulnerable = 6,
        DontSpawnSeagullsOnTheJeep = 7,
        GameIsRunningOnAConsole = 8,
        GameIsRunningOnAPC = 9
    }

    /// <summary>
    /// If set, this specifies a global state to check before firing. The OnMapSpawn output will only fire if the global state is set. <br/>
    /// <b>--- None ---</b> <br/>
    /// <b>Gordon pre-criminal</b> <br/>
    /// <b>Antlions are player allies</b> <br/>
    /// <b>Suit sprint function not yet enabled</b> <br/>
    /// <b>Super phys gun is enabled</b> <br/>
    /// <b>Friendly encounter sequence (lower weapons, etc.)</b> <br/>
    /// <b>Gordon is invulnerable</b> <br/>
    /// <b>Don't spawn seagulls on the jeep</b> <br/>
    /// <b>Game is running on a console</b> <br/>
    /// <b>Game is running on a PC</b> <br/>
    /// </summary>
    [Property(Title = "Global State to Read")]
    public GlobalStateType GlobalState { get; set; } =  GlobalStateType.None;

    /// <summary>
    /// Fired when the map is loaded for any reason.
    /// </summary>
    protected Output OnMapSpawn { get; set; }

    /// <summary>
    /// Fired when the map is loaded to start a new game.
    /// </summary>
    protected Output OnNewGame { get; set; }

    /// <summary>
    /// Fired when the map is loaded from a saved game.
    /// </summary>
    protected Output OnLoadGame { get; set; }

    /// <summary>
    /// Fired when the map is loaded due to a level transition.
    /// </summary>
    protected Output OnMapTransition { get; set; }

    /// <summary>
    /// Fired when the map is loaded as a background to the main menu.
    /// </summary>
    protected Output OnBackgroundMap { get; set; }

    /// <summary>
    ///  Fired only in multiplayer, when a new map is loaded.
    /// </summary>
    protected Output OnMultiNewMap { get; set; }

    /// <summary>
    /// Fired only in multiplayer, when a new round is started. Only fired in multiplayer games that use round-based gameplay.
    /// </summary>
    protected Output OnMultiNewRound { get; set; }

    public override void Spawn()
    {
        base.Spawn();

        // WIP
    }
}

