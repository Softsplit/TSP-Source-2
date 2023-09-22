using Editor;
using Sandbox;

/// <summary>
/// A brush entity for use as a player-useable door.
/// </summary>
[Library("func_door")]
[Solid, HammerEntity, RenderFields, VisGroup(VisGroup.Dynamic)]
[Title("Door"), Category("Function"), Icon("door_front")]
public partial class FuncDoor : ModelEntity
{

}
