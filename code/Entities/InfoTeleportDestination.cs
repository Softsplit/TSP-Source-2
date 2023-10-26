using Editor;
using Sandbox;

/// <summary>
/// An entity that can be used to change the lighting origin of a prop_static. Set the prop_static's Lighting Origin to point at this entity to make the prop_static light as if it was at the info_lighting's origin. Good for prop_static entities that are embedded in world geometry (like rocks/windows/etc).
/// </summary>
[Library("info_teleport_destination"), HammerEntity]
[EditorModel("models/editor/playerstart.vmdl")]
[Title("Teleport Destination"), Category("Information"), Icon("location_on")]
public partial class InfoTeleportDestination : Entity
{

}