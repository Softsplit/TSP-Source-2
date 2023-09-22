using Editor;
using Sandbox;

/// <summary>
/// An entity that issues commands to the client console, as if it was typed in by the player (if activator is a player, or the local player in single player).
/// </summary>
[Library("point_clientcommand"), HammerEntity]
[Title("Client Command"), Category("Point"), Icon("arrow_forward_ios")]
public partial class PointClientcommand : Entity
{

}