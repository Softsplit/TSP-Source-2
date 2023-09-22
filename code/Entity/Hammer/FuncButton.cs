using Editor;
using Sandbox;

/// <summary>
/// A brush entity that's designed to be used for a player-useable button. When used by the player, it moves to a pressed position.
/// </summary>
[Library("func_button")]
[Solid, HammerEntity, RenderFields, VisGroup(VisGroup.Dynamic)]
[Title("Button"), Category("Function"), Icon("radio_button_checked")]
public partial class FuncButton : ModelEntity
{

}
